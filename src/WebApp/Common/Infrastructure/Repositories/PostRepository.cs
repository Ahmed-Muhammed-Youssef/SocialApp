using Application.DTOs.Pagination;
using Application.DTOs.Post;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PostRepository(DataContext _context) : IPostRepository
{
    public async Task<Post> GetByIdAsync(ulong postId, int requesterId)
    {
        return await _context.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId)
    {
        return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Post newPost)
    {
        await _context.Posts.AddAsync(newPost);
    }

    public async Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams)
    {
        var allPostsQuery = _context.Friends
        .Where(f => f.UserId == userId)
        .SelectMany(f => f.FriendUser.Posts, (f, p) => new PostDTO
        {
            Id = p.Id,
            DateEdited = p.DateEdited,
            OwnerName = f.FriendUser.FirstName + " " + f.FriendUser.LastName,
            OwnerId = f.FriendUser.Id,
            Content = p.Content,
            OwnerPictureUrl = f.FriendUser.ProfilePictureUrl,
            DatePosted = p.DatePosted
        })
        .OrderByDescending(p => p.DatePosted)
        .AsNoTracking();

        int count = await allPostsQuery.CountAsync();

        List<PostDTO> posts  = await allPostsQuery.Skip(paginationParams.SkipValue)
            .Take(paginationParams.ItemsPerPage).ToListAsync();

        return new PagedList<PostDTO>(posts, count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
    }
}
