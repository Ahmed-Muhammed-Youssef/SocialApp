namespace Infrastructure.Data.Repositories;

public class PostRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Post>(dataContext), IPostRepository
{
    public async Task<List<PostDTO>> GetUserPostsAsync(int userId, int requesterId)
    {
        return await dataContext.Posts.Where(p => p.UserId == userId)
            .Select(PostMappings.ToPostDTOExpression)
            .ToListAsync();
    }

    public async Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams)
    {
        IQueryable<int> friendsQuery = dataContext.Friends
        .Where(f => f.FriendId == userId).Select(f => f.UserId);

        var allPostsQuery = dataContext.Posts
        .Include(p => p.ApplicationUser)
        .Where(p => friendsQuery.Contains(p.UserId))
        .Select(PostMappings.ToPostDTOExpression)
        .OrderByDescending(p => p.DatePosted)
        .AsNoTracking();

        int count = await allPostsQuery.CountAsync();

        List<PostDTO> posts  = await allPostsQuery.Skip(paginationParams.SkipValue())
            .Take(paginationParams.ItemsPerPage).ToListAsync();

        return new PagedList<PostDTO>(posts, count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
    }
}
