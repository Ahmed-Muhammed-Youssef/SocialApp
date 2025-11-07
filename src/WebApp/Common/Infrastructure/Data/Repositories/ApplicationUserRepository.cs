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
        var projectedQuery = query.Select(u => UserMappings.ToDto(u));

        var count = await projectedQuery.CountAsync();

        // pagination and execution
        List<UserDTO> users = await projectedQuery
            .Skip(userParams.SkipValue())
            .Take(userParams.ItemsPerPage)
            .ToListAsync();

        return new PagedList<UserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }

    public async Task<ApplicationUser?> GetByIdentity(string identity)
    {
        return await dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identity);
    }

    public async Task<UserDTO?> GetDtoByIdentityId(string identityId)
    {
        ApplicationUser? appUser = await dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);

        return appUser is null ? null : UserMappings.ToDto(appUser);
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id)
    {
        var connectionString = dataContext.Database.GetConnectionString();
        using IDbConnection db = new SqlConnection(connectionString);
        return (await db.QueryAsync<UserDTO>("GetUserDtoById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
    }

    public async Task<List<SimplifiedUserDTO>> GetListAsync(int[] ids)
    {
        return await dataContext.ApplicationUsers.Where(u => ids.Contains(u.Id)).Select(u => new SimplifiedUserDTO() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, ProfilePictureUrl = null }).ToListAsync();
    }

    public async Task<SimplifiedUserDTO?> GetSimplifiedDTOAsync(int id)
    {
        return await dataContext.ApplicationUsers.Select(u => new SimplifiedUserDTO() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, ProfilePictureUrl = null }).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<PagedList<SimplifiedUserDTO>> SearchAsync(int userId, string search, UserParams userParams)
    {
        var query = dataContext.ApplicationUsers.Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(search))
            .Select(u => new SimplifiedUserDTO()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePictureUrl = null
            });
        var count = await query.CountAsync();
        var users = await query.Skip(userParams.SkipValue()).Take(userParams.ItemsPerPage).ToListAsync();
        return new PagedList<SimplifiedUserDTO>(users, count, userParams.PageNumber, userParams.ItemsPerPage);
    }
}
