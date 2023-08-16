using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserRepository : IUsersRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public void DeleteUser(AppUser user)
        {
            dataContext.Remove(user);
        }
        public async Task<bool> UserExistsAsync(int id)
        {
            return await dataContext.Users.AnyAsync(e => e.Id == id);
        }
        public void Update(AppUser appUser)
        {
            dataContext.Entry(appUser).State = EntityState.Modified;
        }
        public void UpdatePicture(Picture picture)
        {
            dataContext.Entry(picture).State = EntityState.Modified;
        }
        public async Task<char> GetUserInterest(int userId)
        {
            return await dataContext.Users.Where(u => u.Id == userId)
                .Select(u => u.Interest)
                .FirstOrDefaultAsync();
        }
        public async Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> forbiddenIds)
        {
            var query = dataContext.Users.AsQueryable()
                .Where(u => u.UserName != username);
            if(userParams.Sex != "b")
            {
                query = query.Where(u => u.Sex == userParams.Sex[0]);
            }
            if(userParams.MinAge != null)
            {
                var maxDoB = DateTime.UtcNow.AddYears(-(int)userParams.MinAge);
                query = query.Where(u => u.DateOfBirth <= maxDoB);
            }
            if(userParams.MaxAge != null)
            {
                var minDoB = DateTime.UtcNow.AddYears(-(int)userParams.MaxAge - 1);
                query = query.Where(u => u.DateOfBirth >= minDoB);
            }
            // removes any liked users
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
            
            var queryDto = query.ProjectTo<UserDTO>(mapper.ConfigurationProvider).AsNoTracking();
            var pagedResult = await PagedList<UserDTO>.CreatePageAsync(queryDto, userParams.PageNumber, userParams.ItemsPerPage);
            
            // order the photos according to the order value.
            pagedResult.ForEach(user =>
            {
                user.Pictures = user.Pictures.OrderBy(p => p.Order);
            });
            return pagedResult;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var result = await dataContext.Users
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            return result;
        }
        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            var result = await dataContext.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
            result.Pictures = result.Pictures.OrderBy(p => p.Order);
            return result;
        }
        public async Task<UserDTO> GetUserDTOByUsernameAsync(string username)
        {
            var result =  await dataContext.Users
               .Where(u => u.UserName == username)
               .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
            result.Pictures = result.Pictures.OrderBy(p => p.Order);
            return result;
        } 
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var result = await dataContext.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
            return result;
        }
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var result =  await dataContext.Users
              .Where(u => u.Email == email)
              .FirstOrDefaultAsync();
            result.Pictures = (ICollection<Picture>)result.Pictures.OrderBy(p => p.Order);
            return result;
        }
        public async Task<UserDTO> GetUserDTOByEmailAsync(string email)
        {
            var user = await dataContext.Users
              .Where(u => u.Email == email)
              .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
            user.Pictures = user.Pictures.OrderBy(p => p.Order);
            return user;
        }
        public async Task<IEnumerable<PictureDTO>> GetUserPictureDTOsAsync(int id)
        {
            var result =  await dataContext.Pictures
              .Where(p => p.AppUserId == id)
              .ProjectTo<PictureDTO>(mapper.ConfigurationProvider)
              .ToListAsync();
            return result.OrderBy(p => p.Order);
        }
        public async Task<IEnumerable<Picture>> GetUserPictureAsync(int id)
        {
            var result =  dataContext.Pictures
                .Where(p => p.AppUserId == id)
                .OrderBy(p => p.Order);
            return await result.ToListAsync();
        }
        public async Task<Picture> AddPictureAsync(Picture picture)
        {
            var photos = await GetUserPictureDTOsAsync(picture.AppUserId);
            picture.Order = photos.Count();

            await dataContext.Pictures.AddAsync(picture);
            return picture;
        }
        public void DeletePicture(Picture picture)
        {
            dataContext.Pictures.Remove(picture);
        }
        public async Task<Picture> GetProfilePictureAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if(user == null)
            {
                return null;
            }
            var result = await dataContext.Pictures
            .Where(p => p.AppUserId == userId)
            .FirstOrDefaultAsync(p => p.Order == 0);
            return result;
        }
    }
}
