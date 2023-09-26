using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public void DeleteUser(AppUser user)
        {
            _dataContext.Remove(user);
        }
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _dataContext.Users
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }
        public void Update(AppUser appUser)
        {
            _dataContext.ChangeTracker.Clear();
            _dataContext.Entry(appUser).State = EntityState.Modified;
        }
        public async Task<char> GetUserInterest(int userId)
        {
            return await _dataContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Interest)
                .FirstOrDefaultAsync();
        }
        public async Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> forbiddenIds)
        {
            var query = _dataContext.Users
                .AsNoTracking()
                .AsQueryable()
                .Where(u => u.UserName != username);
            if (userParams.Sex != "b")
            {
                query = query.Where(u => u.Sex == userParams.Sex[0]);
            }
            if (userParams.MinAge != null)
            {
                var maxDoB = DateTime.UtcNow.AddYears(-(int)userParams.MinAge);
                query = query.Where(u => u.DateOfBirth <= maxDoB);
            }
            if (userParams.MaxAge != null)
            {
                var minDoB = DateTime.UtcNow.AddYears(-(int)userParams.MaxAge - 1);
                query = query.Where(u => u.DateOfBirth >= minDoB);
            }
            // removes any friend requested users
            if (forbiddenIds != null || forbiddenIds.Count != 0)
            {
                query = query.Where(u => !forbiddenIds.Contains(u.Id));
            }
            query = userParams.OrderBy switch
            {
                "creationTime" => query.OrderByDescending(u => u.Created),
                "age" => query.OrderByDescending(u => u.DateOfBirth),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            var queryDto = query.ProjectTo<UserDTO>(_mapper.ConfigurationProvider).AsNoTracking();
            var pagedResult = await PagedList<UserDTO>.CreatePageAsync(queryDto, userParams.PageNumber, userParams.ItemsPerPage);

            // @ToDo: order the pictures according to the upload time (Descending).
            return pagedResult;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var result = await _dataContext.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            return result;
        }
        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            var result = await _dataContext.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            // result.Pictures = result.Pictures.OrderBy(p => p.);
            return result;
        }
    }
}
