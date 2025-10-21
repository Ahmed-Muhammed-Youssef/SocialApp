namespace Infrastructure.RealTime.Presence;

public class OnlinePresenceManager
{
    // Dictionary to track online users by their user ID and connection IDs.
    private static readonly Dictionary<int, List<string>> OnlineUsers = [];

    /// <summary>
    /// Adds a connection for a user. If this is the first connection for the user, returns true.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="connectionId">The unique connection ID for this session.</param>
    /// <returns>True if this is the user's first connection; otherwise, false.</returns>
    public Task<bool> UserConnected(int userId, string connectionId)
    {
        var firstConnection = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.TryGetValue(userId, out var userConnections))
            {
                OnlineUsers[userId] = [connectionId];
                firstConnection = true;
            }
            else
            {
                userConnections.Add(connectionId);
            }
        }
        return Task.FromResult(firstConnection);
    }

    /// <summary>
    /// Removes a connection for a user. If this was the user's last connection, returns true.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="connectionId">The unique connection ID for this session.</param>
    /// <returns>True if the user has no remaining connections; otherwise, false.</returns>
    public Task<bool> UserDisconnected(int userId, string connectionId)
    {
        bool isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.TryGetValue(userId, out var userConnections))
            {
                return Task.FromResult(isOffline);
            }

            userConnections.Remove(connectionId);
            if (userConnections.Count == 0)
            {
                OnlineUsers.Remove(userId);
                isOffline = true;
            }
        }
        return Task.FromResult(isOffline);
    }

    /// <summary>
    /// Retrieves a list of currently online users by their user IDs.
    /// </summary>
    /// <returns>An array of user IDs for online users, sorted in ascending order.</returns>
    public Task<int[]> GetOnlineUsers()
    {
        int[] onlineUsers = [];
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(u => u.Key).Select(u => u.Key).ToArray();
        }
        return Task.FromResult(onlineUsers);
    }

    /// <summary>
    /// Retrieves all connection IDs associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    public Task<List<string>> GetConnectionForUser(int userId)
    {
        List<string> connectionIds;
        lock (OnlineUsers)
        {
            connectionIds = OnlineUsers.GetValueOrDefault(userId) ?? [];
        }

        return Task.FromResult(connectionIds);
    }
}
