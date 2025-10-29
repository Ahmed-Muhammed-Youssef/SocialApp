using Application.DTOs.Pagination;
using Application.Features.Users;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
{
    public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
    public Task<UserDTO?> GetDtoByIdentityId(string identityId);
    public Task<ApplicationUser?> GetByIdentity(string identity);
    public Task<UserDTO?> GetDtoByIdAsync(int id);
    public Task<List<SimplifiedUserDTO>> GetListAsync(int[] ids);
    public Task<SimplifiedUserDTO?> GetSimplifiedDTOAsync(int id);

    public Task<PagedList<SimplifiedUserDTO>> SearchAsync(int userId, string search, UserParams userParams);
}
