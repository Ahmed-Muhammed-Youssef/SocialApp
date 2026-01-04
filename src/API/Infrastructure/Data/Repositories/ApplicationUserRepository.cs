namespace Infrastructure.Data.Repositories;

public class ApplicationUserRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<ApplicationUser>(dataContext), IApplicationUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
{  
    public async Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams)
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

        // projection
        var projectedQuery = query.Select(UserMappings.ToDtoExpression);

        var count = await projectedQuery.CountAsync();

        // pagination and execution
        List<UserDTO> users = await projectedQuery
            .Skip(userParams.SkipValue())
            .Take(userParams.ItemsPerPage)
            .ToListAsync();

        return new PagedList<UserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id)
    {
        UserDTO? userDTO = await dataContext.ApplicationUsers.Where(u => u.Id == id)
            .Select(UserMappings.ToDtoExpression)
            .FirstOrDefaultAsync();

        return userDTO;
    }

    public Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId)
    {
        return dataContext.ApplicationUsers
        .Where(u => u.Id == userId && dataContext.UserPictures.Any(up => up.UserId == userId && up.PictureId == pictureId))
        .ExecuteUpdateAsync(s => s.SetProperty(u => u.ProfilePictureId, pictureId));
    }

    public void AddUserPicture(int userId, int pictureId)
    {
        UserPicture userPicture = new() { UserId = userId, PictureId = pictureId };
        dataContext.UserPictures.Add(userPicture);
    }
}
