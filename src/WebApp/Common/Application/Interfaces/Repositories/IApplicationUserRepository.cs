using Application.DTOs.Pagination;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IApplicationUserRepository
    {

        public void Update(ApplicationUser appUser);
        public Task AddAsync(ApplicationUser user);
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
        public Task<UserDTO> GetDtoByIdentityId(string identityId);
        public Task<ApplicationUser> GetByIdentity(string identity);
        public Task<ApplicationUser> GetByIdAsync(int id);
        public Task<UserDTO> GetDtoByIdAsync(int id);
        public void Delete(ApplicationUser user);
        public Task<bool> IdExistsAsync(int id);
        public Task<List<SimplifiedUserDTO>> GetListAsync(int[] ids);
        public Task<SimplifiedUserDTO> GetSimplifiedDTOAsync(int id);

        public Task<PagedList<SimplifiedUserDTO>> SearchAsync(int userId, string search, UserParams userParams);
    }
}
