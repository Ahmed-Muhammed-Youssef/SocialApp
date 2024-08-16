using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext _context) : IPostRepository
    {
        public async Task<Post> GetPostByIdAsync(ulong postId, int requesterId)
        {
            return await _context.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId)
        {
            return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task AddPostAsync(Post newPost)
        {
            await _context.Posts.AddAsync(newPost);
        }
    }
}
