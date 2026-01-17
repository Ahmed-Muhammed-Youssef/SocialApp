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
        UserPicture userPicture = new() { UserId = userId, PictureId = pictureId };
        dataContext.UserPictures.Add(userPicture);
    }
}
