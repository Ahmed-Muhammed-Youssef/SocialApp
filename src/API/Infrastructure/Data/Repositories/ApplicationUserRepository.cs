namespace Infrastructure.Data.Repositories;

public class ApplicationUserRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<ApplicationUser>(dataContext), IApplicationUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
{
    public async Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams, CancellationToken cancellationToken = default)
    {
        DateTime? birthDateMax = userParams.MaxAge != null ? DateTime.UtcNow.AddYears(-userParams.MaxAge.Value - 1) : null;
        DateTime birthDateMin = DateTime.UtcNow.AddYears(-userParams.MinAge - 1);

        IQueryable<ApplicationUser> query = dataContext.ApplicationUsers
            .Where(u => u.Id != userId && u.DateOfBirth >= birthDateMin && (birthDateMax == null || u.DateOfBirth <= birthDateMax))
            .AsQueryable();

        // filteration based on relation
        if (userParams.RelationFilter == RelationFilter.OnlyFriends)
        {
            IQueryable<int> friendsQuery = dataContext.Friends.Where(f => f.UserId == userId).Select(f => f.FriendId);
            query = query.Where(u => friendsQuery.Contains(u.Id));
        }
        else if (userParams.RelationFilter == RelationFilter.OnlyFriendRequested)
        {
            IQueryable<int> friendRequestedQuery = dataContext.FriendRequests.Where(fr => fr.RequesterId == userId).Select(f => f.RequesterId);
            query = query.Where(u => friendRequestedQuery.Contains(u.Id));
        }
        else if (userParams.RelationFilter == RelationFilter.ExcludeFriendsAndFriendRequested)
        {
            IQueryable<int> friendsQuery = dataContext.Friends.Where(f => f.UserId == userId).Select(f => f.FriendId);
            IQueryable<int> friendRequestedQuery = dataContext.FriendRequests.Where(fr => fr.RequesterId == userId).Select(f => f.RequesterId);
            query = query.Where(u => !friendsQuery.Contains(u.Id) && !friendRequestedQuery.Contains(u.Id));
        }

        // ordering
        query = userParams.OrderBy switch
        {
            OrderByOptions.CreationTime => query.OrderByDescending(u => u.Created),
            OrderByOptions.LastActive => query.OrderByDescending(u => u.LastActive),
            OrderByOptions.Age => query.OrderByDescending(u => u.DateOfBirth),
            _ => query.OrderByDescending(u => u.LastActive)
        };

        var count = await query.CountAsync(cancellationToken: cancellationToken);

        var projectedQuery = query.LeftJoin(dataContext.Pictures,
                                u => u.ProfilePictureId,
                                p => p.Id,
                                UserMappings.ToDtoWithPictureExpression);

        // pagination and execution
        List<UserDTO> users = await projectedQuery
            .Skip(userParams.SkipValue())
            .Take(userParams.ItemsPerPage)
            .ToListAsync(cancellationToken: cancellationToken);

        return new PagedList<UserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        UserDTO? userDTO = await dataContext.ApplicationUsers
            .Where(u => u.Id == id)
            .LeftJoin(dataContext.Pictures,
                u => u.ProfilePictureId,
                p => p.Id,
                UserMappings.ToDtoWithPictureExpression)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return userDTO;
    }

    public async Task<UserDTO?> GetDtoByIdentityAsync(string identityId, CancellationToken cancellationToken = default)
    {
        UserDTO? userDTO = await dataContext.ApplicationUsers
            .Where(u => u.IdentityId == identityId)
            .LeftJoin(dataContext.Pictures,
                u => u.ProfilePictureId,
                p => p.Id,
                UserMappings.ToDtoWithPictureExpression)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return userDTO;
    }

    public Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId, CancellationToken cancellationToken = default)
    {
        return dataContext.ApplicationUsers
        .Where(u => u.Id == userId && dataContext.UserPictures.Any(up => up.UserId == userId && up.PictureId == pictureId))
        .ExecuteUpdateAsync(s => s.SetProperty(u => u.ProfilePictureId, pictureId), cancellationToken: cancellationToken);
    }

    public void AddUserPicture(int userId, int pictureId)
    {
        UserPicture userPicture = new(userId, pictureId);
        dataContext.UserPictures.Add(userPicture);
    }

    // Post-related methods
    public void AddPost(Post post)
    {
        dataContext.Posts.Add(post);
    }

    public async Task<Post?> GetPostByIdAsync(ulong postId, CancellationToken cancellationToken = default)
    {
        return await dataContext.Posts
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
    }

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

    // Picture-related queries
    public async Task<List<Picture>> GetUserPicturesAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await dataContext.UserPictures
            .Where(up => up.UserId == userId)
            .Join(dataContext.Pictures, up => up.PictureId, p => p.Id, (up, p) => p)
            .OrderByDescending(p => p.Created)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PictureDTO>> GetUserPictureDTOsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await dataContext.UserPictures
            .Where(up => up.UserId == userId)
            .Join(dataContext.Pictures, up => up.PictureId, p => p.Id, (up, p) => p)
            .OrderByDescending(p => p.Created)
            .Select(p => new PictureDTO(p.Id, p.Url))
            .ToListAsync(cancellationToken);
    }

    public async Task<PictureDTO?> GetUserPictureDTOAsync(int userId, int pictureId, CancellationToken cancellationToken = default)
    {
        return await dataContext.UserPictures
            .Where(up => up.UserId == userId && up.PictureId == pictureId)
            .Join(dataContext.Pictures, up => up.PictureId, p => p.Id, (up, p) => p)
            .Select(PictureMappings.ToDtoExpression)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
