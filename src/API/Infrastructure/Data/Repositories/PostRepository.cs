namespace Infrastructure.Data.Repositories;

public class PostRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Post>(dataContext), IPostRepository
{
    public async Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int requesterId)
    {
        return await dataContext.Posts.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams)
    {
        IQueryable<int> friendsQuery = dataContext.Friends
        .Where(f => f.FriendId == userId).Select(f => f.UserId);

        var allPostsQuery = dataContext.Posts
        .Include(p => p.ApplicationUser)
        .Where(p => friendsQuery.Contains(p.UserId))
        .Select(p => new PostDTO
        {
            Id = p.Id,
            DateEdited = p.DateEdited,
            OwnerName = p.ApplicationUser!.FirstName + " " + p.ApplicationUser!.LastName,
            OwnerId = p.UserId,
            Content = p.Content,
            OwnerPictureUrl = "",
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
