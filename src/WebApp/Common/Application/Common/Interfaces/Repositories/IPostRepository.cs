using Application.DTOs.Pagination;
using Application.Features.Posts;
using Domain.Entities;
using Shared.Pagination;

namespace Application.Common.Interfaces.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task<Post?> GetByIdAsync(ulong postId);
    Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId);
    Task AddAsync(Post newPost);
    Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams);
}
