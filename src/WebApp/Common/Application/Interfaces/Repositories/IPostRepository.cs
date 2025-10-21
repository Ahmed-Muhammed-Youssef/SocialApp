using Application.DTOs.Pagination;
using Application.Features.Posts;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(ulong postId, int requesterId);
    Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId);
    Task AddAsync(Post newPost);
    Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams);
}
