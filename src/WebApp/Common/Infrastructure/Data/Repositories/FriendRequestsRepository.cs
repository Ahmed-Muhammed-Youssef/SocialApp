namespace Infrastructure.Data.Repositories;

public class FriendRequestsRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<FriendRequest>(dataContext), IFriendRequestRepository
{
    public async Task<bool> IsFriend(int userId, int targetId)
    {
        return await dataContext.Friends
            .AsNoTracking()
            .AnyAsync(u => u.UserId == userId && u.FriendId == targetId);
    }

    public async Task<FriendRequest?> GetFriendRequestAsync(int senderId, int targetId)
    {
        return await dataContext.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.RequestedId == targetId && fr.RequesterId == senderId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsFriendRequestedAsync(int senderId, int targetId)
    {
        return await dataContext.FriendRequests
            .AsNoTracking()
            .AnyAsync(fr => fr.RequestedId == targetId && fr.RequesterId == senderId);
    }

    public async Task<List<UserDTO>> GetRecievedFriendRequestsAsync(int targetId)
    {
        return await dataContext.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.RequestedId == targetId && fr.Status == RequestStatus.Pending)
            .Join(
                dataContext.ApplicationUsers,
                fr => fr.RequesterId,
                user => user.Id,
                (fr, user) => user)
            .Select(u => UserMappings.ToDto(u))
            .ToListAsync();
    }
    public async Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId)
    {
        IEnumerable<UserDTO> users = [];

        using (var connection = new SqlConnection(dataContext.Database.GetDbConnection().ConnectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@senderId", senderId);

            users = await connection.QueryAsync<UserDTO>("GetFriendRequestedUsersDTO", parameters, commandType: CommandType.StoredProcedure);
        }
        return users;
    }
}
