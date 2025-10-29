using Application.Common.Interfaces.Repositories;
using Application.DTOs.Pagination;
using Application.Features.Posts;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;

namespace Infrastructure.Repositories;

public class PostRepository(DataContext dataContext) : RepositoryBase<Post>(dataContext), IPostRepository
{
    public async Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId)
    {
        return await dataContext.Posts.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams)
    {
        var allPostsQuery = dataContext.Friends
        .Where(f => f.UserId == userId)
        .SelectMany(f => f.FriendUser!.Posts, (f, p) => new PostDTO
        {
            Id = p.Id,
            DateEdited = p.DateEdited,
            OwnerName = f.FriendUser!.FirstName + " " + f.FriendUser.LastName,
            OwnerId = f.FriendUser.Id,
            Content = p.Content,
            OwnerPictureUrl = f.FriendUser!.ProfilePictureUrl ?? "",
            DatePosted = p.DatePosted
        })
        .OrderByDescending(p => p.DatePosted)
        .AsNoTracking();

        int count = await allPostsQuery.CountAsync();

        List<PostDTO> posts  = await allPostsQuery.Skip(paginationParams.SkipValue())
            .Take(paginationParams.ItemsPerPage).ToListAsync();

        return new PagedList<PostDTO>(posts, count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
    }
}
