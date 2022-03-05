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
        public async Task<PagedList<UserDTO>> GetUsersDTOAsync(UserParams userParams)
        {
            var query = dataContext.Users
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).AsNoTracking();
            query.Skip(userParams.PageNumber * userParams.ItemsPerPage).Take(userParams.ItemsPerPage);
            var pagedResult = await PagedList<UserDTO>.CreatePageAsync(query, userParams.PageNumber, userParams.ItemsPerPage);
            
            // order the photos according to the order value.
            pagedResult.ForEach(user =>
            {
                user.Photos = user.Photos.OrderBy(p => p.Order);
            });
            return pagedResult;
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

    }
}
