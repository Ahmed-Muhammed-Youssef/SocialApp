namespace Application.Common.Interfaces.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task<List<PostDTO>> GetUserPostsAsync(int userId, CancellationToken cancellationToken = default);
    Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams, CancellationToken cancellationToken = default);
}
