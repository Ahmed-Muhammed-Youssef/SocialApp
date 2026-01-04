namespace Application.Common.Interfaces.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
{
    Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams);
    Task<UserDTO?> GetDtoByIdAsync(int id);
    Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId);
    void AddUserPicture(int userId, int pictureId);
}
