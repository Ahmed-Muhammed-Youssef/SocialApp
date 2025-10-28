using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Application.DTOs.Pagination;
using AutoMapper;
using Application.Features.Users;
using Domain.Enums;
using Shared.Extensions;
using Application.Features.Pictures;
using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class ApplicationUserRepository(DataContext _dataContext, IMapper mapper) : IApplicationUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
{
    public void Delete(ApplicationUser user)
    {
        _dataContext.Remove(user);
    }

    public async Task AddAsync(ApplicationUser user)
    {
        await _dataContext.ApplicationUsers.AddAsync(user);
    }

    public async Task<bool> IdExistsAsync(int id)
    {
        return await _dataContext.ApplicationUsers
            .AsNoTracking()
            .AnyAsync(e => e.Id == id);
    }
    
    public void Update(ApplicationUser appUser)
    {
        _dataContext.ChangeTracker.Clear();
        _dataContext.Entry(appUser).State = EntityState.Modified;
    }

    public async Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams)
    {
        DateTime? birthDateMax = userParams.MaxAge != null ? DateTime.UtcNow.AddYears(-userParams.MaxAge.Value - 1) : null;
        DateTime birthDateMin = DateTime.UtcNow.AddYears(-userParams.MinAge - 1);

        IQueryable<ApplicationUser> query = _dataContext.ApplicationUsers
            .Include(u => u.Friends)
            .Include(u => u.FriendRequestsReceived)
            .Where(u => u.Id != userId && u.DateOfBirth >= birthDateMin && (birthDateMax == null || u.DateOfBirth <= birthDateMax))
            .AsQueryable();

        // filteration based on relation
        if (userParams.RelationFilter == RelationFilter.OnlyFriends)
        {
            query = query.Where(u => u.Friends.Any(f => f.FriendId == userId));
        }
        else if (userParams.RelationFilter == RelationFilter.OnlyFriendRequested)
        {
            query = query.Where(u => u.FriendRequestsReceived.Any(fr => fr.RequesterId == userId));
        }
        else if (userParams.RelationFilter == RelationFilter.ExcludeFriendsAndFriendRequested)
        {
            query = query.Where(u => !u.Friends.Any(f => f.FriendId == userId) &&
                                     !u.FriendRequestsReceived.Any(fr => fr.RequesterId == userId));
        }

        // ordering
        query = userParams.OrderBy switch
        {
            OrderByOptions.CreationTime => query.OrderByDescending(u => u.Created),
            OrderByOptions.LastActive => query.OrderByDescending(u => u.LastActive),
            OrderByOptions.Age => query.OrderByDescending(u => u.DateOfBirth),
            _ => query.OrderByDescending(u => u.LastActive)
        };

        // projection
        var projectedQuery = query.Select(u => new UserDTO
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            ProfilePictureUrl = u.ProfilePictureUrl,
            Sex = u.Sex,
            Age = u.DateOfBirth.CalculateAge(),
            Created = u.Created,
            LastActive = u.LastActive,
            Bio = u.Bio ?? string.Empty,
            Pictures = u.Pictures.Select(p => new PictureDTO(p.Id, p.Url))
        });

        var count = await projectedQuery.CountAsync();

        // pagination and execution
        List<UserDTO> users = await projectedQuery
            .Skip(userParams.SkipValue())
            .Take(userParams.ItemsPerPage)
            .ToListAsync();

        return new PagedList<UserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }

    public async Task<ApplicationUser?> GetByIdentity(string identity)
    {
        return await _dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identity);
    }

    public async Task<ApplicationUser?> GetByIdAsync(int id)
    {
        var connectionString = _dataContext.Database.GetConnectionString();
        using IDbConnection db = new SqlConnection(connectionString);
        return (await db.QueryAsync<ApplicationUser>("GetUserById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
    }

    public async Task<UserDTO?> GetDtoByIdentityId(string identityId)
    {
        var appUser = await _dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);

        return mapper.Map<UserDTO>(appUser);
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id)
    {
        var connectionString = _dataContext.Database.GetConnectionString();
        using IDbConnection db = new SqlConnection(connectionString);
        return (await db.QueryAsync<UserDTO>("GetUserDtoById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
    }

    public async Task<List<SimplifiedUserDTO>> GetListAsync(int[] ids)
    {
        return await _dataContext.ApplicationUsers.Where(u => ids.Contains(u.Id)).Select(u => new SimplifiedUserDTO() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, ProfilePictureUrl = u.ProfilePictureUrl }).ToListAsync();
    }

    public async Task<SimplifiedUserDTO?> GetSimplifiedDTOAsync(int id)
    {
        return await _dataContext.ApplicationUsers.Select(u => new SimplifiedUserDTO() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, ProfilePictureUrl = u.ProfilePictureUrl }).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<PagedList<SimplifiedUserDTO>> SearchAsync(int userId, string search, UserParams userParams)
    {
        var query = _dataContext.ApplicationUsers.Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(search))
            .Select(u => new SimplifiedUserDTO()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePictureUrl = u.ProfilePictureUrl
            });
        var count = await query.CountAsync();
        var users = await query.Skip(userParams.SkipValue()).Take(userParams.ItemsPerPage).ToListAsync();
        return new PagedList<SimplifiedUserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }
}
