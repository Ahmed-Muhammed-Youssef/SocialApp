namespace Application.Common.Interfaces.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
{
    public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
    public Task<UserDTO?> GetDtoByIdAsync(int id);
}
