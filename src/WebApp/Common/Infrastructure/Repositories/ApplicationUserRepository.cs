using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Application.DTOs.User;
using Application.DTOs.Pagination;
using AutoMapper;
using Application.Features.Users;

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
        IEnumerable<UserDTO> users = [];

        using (var connection = new SqlConnection(_dataContext.Database.GetDbConnection().ConnectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            parameters.Add("@minAge", userParams.MinAge);
            parameters.Add("@maxAge", userParams.MaxAge);
            parameters.Add("@orderBy", (int)userParams.OrderBy);
            parameters.Add("@pageNumber", userParams.PageNumber);
            parameters.Add("@pageSize", userParams.ItemsPerPage);

            users = await connection.QueryAsync<UserDTO>("GetUsersDtos", parameters, commandType: CommandType.StoredProcedure);
        }
        return new PagedList<UserDTO>(users.ToList(), users.Count(), userParams.PageNumber, userParams.ItemsPerPage);
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
        return await _dataContext.ApplicationUsers.Where(u => ids.Contains(u.Id)).Select(u => new SimplifiedUserDTO() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, ProfilePictureUrl = u.ProfilePictureUrl} ).ToListAsync();
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
        var users = await query.Skip(userParams.SkipValue).Take(userParams.ItemsPerPage).ToListAsync();
        return new PagedList<SimplifiedUserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }
}
