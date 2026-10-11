namespace Notifications.API.Channels.SignalR;

/// <summary>
/// Tracks which users have at least one open connection to <see cref="NotificationsHub"/> on this instance.
/// In-memory, so it assumes a single instance; scaling out needs a Redis backplane and shared presence.
/// </summary>
public sealed class PresenceTracker
{
    private readonly Dictionary<string, int> _connections = new(StringComparer.Ordinal);
    private readonly Lock _lock = new();

    /// <returns><see langword="true"/> if this is the user's first open connection.</returns>
    public bool Connect(string userId)
    {
        lock (_lock)
        {
            int count = _connections.GetValueOrDefault(userId) + 1;
            _connections[userId] = count;
            return count == 1;
        }
    }

    public void Disconnect(string userId)
    {
        lock (_lock)
        {
            if (!_connections.TryGetValue(userId, out int count))
            {
                return;
            }

            if (count <= 1)
            {
                _connections.Remove(userId);
            }
            else
            {
                _connections[userId] = count - 1;
            }
        }
    }

    public bool IsOnline(string userId)
    {
        lock (_lock)
        {
            return _connections.ContainsKey(userId);
        }
    }
}
