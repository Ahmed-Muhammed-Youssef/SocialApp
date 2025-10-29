using Application.Features.Messages;

namespace Infrastructure.RealTime.Presence;

/// <summary>
/// Manages the online presence of users by tracking their connection states and IDs.
/// </summary>
/// <remarks>This class provides methods to handle user connections and disconnections, retrieve the list of
/// online users, and obtain connection IDs for a specific user. It ensures thread-safe operations on the user
/// connection data.</remarks>
public class OnlinePresenceManager : IOnlinePresenceManager
{
    // Dictionary to track online users by their user ID and connection IDs.
    private static readonly Dictionary<int, List<string>> OnlineUsers = [];

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public Task<int[]> GetOnlineUsers()
    {
        int[] onlineUsers = [];
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(u => u.Key).Select(u => u.Key).ToArray();
        }
        return Task.FromResult(onlineUsers);
    }

    /// <inheritdoc/>
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
