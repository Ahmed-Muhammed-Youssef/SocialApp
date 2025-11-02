namespace Infrastructure.Data.Repositories;

public class FriendRequestsRepository(ApplicationDatabaseContext dataContext) : RepositoryBase<FriendRequest>(dataContext), IFriendRequestRepository
{
    public async Task<bool> SendFriendRequest(int senderId, int targetId)
    {
        var frFromTarget = await dataContext.FriendRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(fr => fr.RequesterId == targetId && fr.RequestedId == senderId);
        if (frFromTarget != null)
        {
            await dataContext.Friends.AddAsync(new Friend { UserId = senderId, FriendId = targetId });
            await dataContext.Friends.AddAsync(new Friend { UserId = targetId, FriendId = senderId });
            dataContext.FriendRequests.Remove(frFromTarget);
        }
        else
        {
            await dataContext.FriendRequests.AddAsync(new FriendRequest()
            {
                RequesterId = senderId,
                RequestedId = targetId
            });
        }
        return frFromTarget != null;
    }

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
            .Include(fr => fr.Requester)
            .Where(fr => fr.RequestedId == targetId)
            .Select(fr => UserMappings.ToDto(fr.Requester!))
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
