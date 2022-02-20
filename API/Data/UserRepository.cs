using API.DTOs;
using API.Entities;
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

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var appUser = await dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var photos = await GetUserPhotosAsync(appUser.Id);
            appUser.Photos = photos.ToList();
            return appUser;
        }

        public async Task<IEnumerable<Photo>> GetUserPhotosAsync(int id)
        {
            return await dataContext.Photo.Where(p => p.AppUserId == id).ToListAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetUsersDTOAsync()
        {
            return await dataContext.Users
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            return await dataContext.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<UserDTO> GetUserDTOByUsernameAsync(string username)
        {
            return await dataContext.Users
               .Where(u => u.UserName == username)
               .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<UserDTO> GetUserDTOByEmailAsync(string email)
        {
            return await dataContext.Users
              .Where(u => u.Email == email)
              .ProjectTo<UserDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(); ;
        }

        public async Task<IEnumerable<PhotoSentDTO>> GetUserPhotoDTOsAsync(int id)
        {
            return await dataContext.Photo
              .Where(p => p.AppUserId == id)
              .ProjectTo<PhotoSentDTO>(mapper.ConfigurationProvider)
              .ToListAsync();
        }
    }
}
