namespace Infrastructure.Users;

/// <summary>
/// Manages the online presence of users by tracking their connection states and IDs.
/// </summary>
/// <remarks>This class provides methods to handle user connections and disconnections, retrieve the list of
/// online users, and obtain connection IDs for a specific user. It ensures thread-safe operations on the user
/// connection data.</remarks>
public class OnlineUsersStore : IOnlineUsersStore
{
    // Dictionary to track online users by their user ID and connection IDs.
    private readonly ConcurrentDictionary<int, HashSet<string>> OnlineUsers = [];

    /// <inheritdoc/>
    public Task<bool> AddUserConnection(int userId, string connectionId)
    {
        var connections = OnlineUsers.GetOrAdd(userId, _ => []);

        lock (connections)
        {
            bool wasOffline = connections.Count == 0;
            connections.Add(connectionId);
            return Task.FromResult(wasOffline);
        }
    }

    /// <inheritdoc/>
    public Task<bool> RemoveUserConnection(int userId, string connectionId)
    {
        if (!OnlineUsers.TryGetValue(userId, out var connections))
            return Task.FromResult(false);

        lock (connections)
        {
            connections.Remove(connectionId);

            if (connections.Count == 0)
            {
                OnlineUsers.TryRemove(userId, out _);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task<int[]> GetOnlineUsers()
    {
        return Task.FromResult(OnlineUsers.Keys.OrderBy(id => id).ToArray());
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<string>> GetConnectionsByUserId(int userId)
    {
        IReadOnlyList<string> connectionIds = OnlineUsers.GetValueOrDefault(userId)?.ToList() ?? [];
        return Task.FromResult(connectionIds);
    }
}
