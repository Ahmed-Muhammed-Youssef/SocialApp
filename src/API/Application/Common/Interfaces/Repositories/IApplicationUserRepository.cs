namespace Application.Common.Interfaces.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
{
    Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams, CancellationToken cancellationToken = default);
    Task<UserDTO?> GetDtoByIdentityAsync(string identityId, CancellationToken cancellationToken = default);
    Task<UserDTO?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId, CancellationToken cancellationToken = default);
    void AddUserPicture(int userId, int pictureId);
}
