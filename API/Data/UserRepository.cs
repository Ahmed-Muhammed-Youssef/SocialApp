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
    public class UserRepository : IUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
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
        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0; 
        }
        public async Task<bool> UserExistsAsync(int id)
        {
            return await dataContext.Users.AnyAsync(e => e.Id == id);
        }
        public void Update(AppUser appUser)
        {
            dataContext.Entry(appUser).State = EntityState.Modified;
        }
        public void UpdatePhoto(Photo photo)
        {
            dataContext.Entry(photo).State = EntityState.Modified;
        }
        public async Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> forbiddenIds)
        {
            var query = dataContext.Users.AsQueryable().Where(u => u.UserName != username);
            if(userParams.Sex != "b")
            {
                query = query.Where(u => u.Sex == userParams.Sex[0]);
            }
            if(userParams.MinAge != null)
            {
                var maxDoB = DateTime.Now.AddYears(-(int)userParams.MinAge);
                query = query.Where(u => u.DateOfBirth <= maxDoB);
            }
            if(userParams.MaxAge != null)
            {
                var minDoB = DateTime.Now.AddYears(-(int)userParams.MaxAge - 1);
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
                user.Photos = user.Photos.OrderBy(p => p.Order);
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
            result.Photos = result.Photos.OrderBy(p => p.Order);
            return result;
        }
        public async Task<UserDTO> GetUserDTOByUsernameAsync(string username)
        {
            var result =  await dataContext.Users
               .Where(u => u.UserName == username)
               .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
            result.Photos = result.Photos.OrderBy(p => p.Order);
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
            result.Photos = (ICollection<Photo>)result.Photos.OrderBy(p => p.Order);
            return result;
        }
        public async Task<UserDTO> GetUserDTOByEmailAsync(string email)
        {
            var user = await dataContext.Users
              .Where(u => u.Email == email)
              .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
            user.Photos = user.Photos.OrderBy(p => p.Order);
            return user;
        }
        public async Task<IEnumerable<PhotoDTO>> GetUserPhotoDTOsAsync(int id)
        {
            var result =  await dataContext.Photo
              .Where(p => p.AppUserId == id)
              .ProjectTo<PhotoDTO>(mapper.ConfigurationProvider)
              .ToListAsync();
            return result.OrderBy(p => p.Order);
        }
        public async Task<IEnumerable<Photo>> GetUserPhotoAsync(int id)
        {
            var result =  dataContext.Photo.Where(p => p.AppUserId == id).OrderBy(p => p.Order);
            return await result.ToListAsync();
        }
        public async Task<Photo> AddPhotoAsync(Photo photo)
        {
            var photos = await GetUserPhotoDTOsAsync(photo.AppUserId);
            photo.Order = photos.Count();

            await dataContext.Photo.AddAsync(photo);
            return photo;
        }
        public void DeletePhoto(Photo photo)
        {
            dataContext.Photo.Remove(photo);
        }
        public async Task<Photo> GetProfilePhotoAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if(user == null)
            {
                return null;
            }
            var result = await dataContext.Photo
            .Where(p => p.AppUserId == userId)
            .FirstOrDefaultAsync(p => p.Order == 0);
            return result;
        }
    }
}
