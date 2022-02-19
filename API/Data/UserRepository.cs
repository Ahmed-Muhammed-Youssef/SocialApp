using API.Entities;
using API.Interfaces;
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

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void DeleteUser(AppUser user)
        {
            dataContext.Remove(user);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var appUser = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            var photos = await GetUserPhotosAsync(appUser.Id);
            appUser.Photos = photos.ToList();
            return appUser;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var appUser = await dataContext.Users.FindAsync(id);
            var photos = await GetUserPhotosAsync(appUser.Id);
            appUser.Photos = photos.ToList();
            return appUser;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await dataContext.Users.ToListAsync();
            users.ForEach(async u =>
            {
                var photos = await GetUserPhotosAsync(u.Id);
                u.Photos = photos.ToList();
            });
            return users;
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
    }
}
