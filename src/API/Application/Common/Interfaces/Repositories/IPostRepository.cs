namespace Application.Common.Interfaces.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId);
    Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams);
}
