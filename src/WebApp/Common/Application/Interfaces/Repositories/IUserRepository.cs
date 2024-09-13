using Application.DTOs.Pagination;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {

        public void Update(ApplicationUser appUser);
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
        public Task<ApplicationUser> GetUserByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByIdAsync(int id);
        public void DeleteUser(ApplicationUser user);
        public Task<bool> UserExistsAsync(int id);
    }
}
