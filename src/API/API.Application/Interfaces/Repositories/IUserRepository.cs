using API.Application.DTOs;
using API.Domain.Entities;

namespace API.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public void Update(AppUser appUser);
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByIdAsync(int id);
        public void DeleteUser(AppUser user);
        public Task<bool> UserExistsAsync(int id);
    }
}
