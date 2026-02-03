namespace Application.Common.Interfaces.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
{
    Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams, CancellationToken cancellationToken = default);
    Task<UserDTO?> GetDtoByIdentityAsync(string identityId, CancellationToken cancellationToken = default);
    Task<UserDTO?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId, CancellationToken cancellationToken = default);
    void AddUserPicture(int userId, int pictureId);

    // Post-related methods
    void AddPost(Post post);
    Task<Post?> GetPostByIdAsync(ulong postId, CancellationToken cancellationToken = default);
    Task<List<PostDTO>> GetUserPostsAsync(int userId, CancellationToken cancellationToken = default);
    Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams, CancellationToken cancellationToken = default);
    // Picture-related queries
    Task<List<Picture>> GetUserPicturesAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<PictureDTO>> GetUserPictureDTOsAsync(int userId, CancellationToken cancellationToken = default);
    Task<PictureDTO?> GetUserPictureDTOAsync(int userId, int pictureId, CancellationToken cancellationToken = default);
}
