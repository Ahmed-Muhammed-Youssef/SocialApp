using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        public void Update(AppUser appUser);
        public Task<bool> SaveAllAsync();
        public Task<IEnumerable<UserDTO>> GetUsersDTOAsync();
        public Task<UserDTO> GetUserDTOByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByUsernameAsync(string username);
        public Task<UserDTO> GetUserDTOByEmailAsync(string email);
        public void DeleteUser(AppUser user);
        public Task<bool> UserExistsAsync(int id);
        public Task<IEnumerable<PhotoSentDTO>> GetUserPhotoDTOsAsync(int id);

        public Task<AppUser> GetUserByEmailAsync(string email);
    }
}
