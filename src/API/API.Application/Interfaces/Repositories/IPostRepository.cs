using API.Domain.Entities;

namespace API.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(ulong postId, int requesterId);
        Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId);
        Task AddPostAsync(Post newPost);
    }
}
