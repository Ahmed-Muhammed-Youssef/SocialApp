namespace Infrastructure.DirectChats;

/// <inheritdoc/>
public class DirectChatGroupsStore : IDirectChatGroupStore
{
    private readonly ConcurrentDictionary<string, Group> _groups = [];

    /// <inheritdoc/>
    public Group AddConnection(int issuerId, int otherUserId, string connectionId)
    {
        string groupName = GetGroupName(issuerId, otherUserId);

        Group group = _groups.GetOrAdd(groupName, _ => new Group(groupName, []));

        lock (group.Connections)
        {
            group.Connections.Add(new Connection(connectionId, issuerId));
        }

        return group;
    }

    /// <inheritdoc/>
    public Group? RemoveConnection(string connectionId)
    {
        foreach (var kvp in _groups)
        {
            var group = kvp.Value;

            lock (group.Connections)
            {
                Connection? connection = group.Connections.FirstOrDefault(c => c.ConnectionId == connectionId);
                if (connection != null)
                {
                    group.Connections.Remove(connection);

                    // Clean up empty groups for memory efficiency
                    if (group.Connections.Count == 0)
                    {
                        _groups.TryRemove(kvp.Key, out _);
                    }

                    return group;
                }
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public Group GetOrAddGroup(int issuerId, int otherUserId)
    {
        string groupName = GetGroupName(issuerId, otherUserId);

        Group group = _groups.GetOrAdd(groupName, _ => new Group(groupName, []));

        return group;
    }

    /// <summary>
    /// Builds a deterministic group name based on two user IDs.
    /// </summary>
    /// <param name="userId1">The first user's ID.</param>
    /// <param name="userId2">The second user's ID.</param>
    /// <returns>
    /// A consistent group name such as "3-7", where the smaller ID comes first.
    /// </returns>
    private static string GetGroupName(int userId1, int userId2)
    {
        return userId1 < userId2
            ? $"{userId1}-{userId2}"
            : $"{userId2}-{userId1}";
    }
}
