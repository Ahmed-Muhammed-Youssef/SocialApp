using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        public void Update(AppUser appUser);
        public Task<bool> SaveAllAsync();
        public Task<IEnumerable<AppUser>> GetUsersAsync();
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> GetUserByUsernameAsync(string username);
        public Task<AppUser> GetUserByEmailAsync(string email);
        public void DeleteUser(AppUser user);
        public Task<bool> UserExistsAsync(int id);
        public Task<IEnumerable<Photo>> GetUserPhotosAsync(int id);
    }
}
