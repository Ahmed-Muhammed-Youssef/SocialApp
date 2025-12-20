namespace Infrastructure.Data.Repositories;

public class PostRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<Post>(dataContext), IPostRepository
{
    public async Task<List<PostDTO>> GetUserPostsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await dataContext.Posts.Where(p => p.UserId == userId)
            .Select(PostMappings.ToPostDTOExpression)
            .OrderByDescending(p => p.DatePosted)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<PagedList<PostDTO>> GetNewsfeed(int userId, PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        IQueryable<int> friendsQuery = dataContext.Friends
        .Where(f => f.FriendId == userId || f.UserId == userId).Select(f => f.UserId == userId ? f.FriendId : f.UserId);

        var allPostsQuery = dataContext.Posts
            .Include(p => p.ApplicationUser)
            .Where(p => friendsQuery.Contains(p.UserId))
            .Select(PostMappings.ToPostDTOExpression)
            .OrderByDescending(p => p.DatePosted)
            .AsNoTracking();

        int count = await allPostsQuery.CountAsync(cancellationToken: cancellationToken);

        List<PostDTO> posts = await allPostsQuery.Skip(paginationParams.SkipValue())
            .Take(paginationParams.ItemsPerPage).ToListAsync(cancellationToken: cancellationToken);

        return new PagedList<PostDTO>(posts, count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
    }
}
