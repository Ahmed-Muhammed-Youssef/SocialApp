namespace Application.Features.DirectChats.Stores;

/// <summary>
/// Provides an in-memory store for tracking user-to-user chat groups and their active connections.
/// </summary>
/// <remarks>
/// This store maintains a mapping between generated group names and their associated <see cref="Group"/> instances.
/// Each <see cref="Group"/> contains one or more active <see cref="Connection"/> objects.
/// </remarks>
public interface IDirectChatGroupsStore
{
    /// <summary>
    /// Retrieves an existing group shared between two users, if it doesn't exist, it creates a new group.
    /// </summary>
    /// <param name="issuerId">The ID of the user making the request (e.g., the current or sender user).</param>
    /// <param name="otherUserId">The ID of the other user participating in the chat.</param>
    /// <returns>
    /// The existing <see cref="Group"/> instance if found; otherwise, it returnes the new <see cref="Group"/> instance.
    /// </returns>
    public Group GetOrAddGroup(int issuerId, int otherUserId);

    /// <summary>
    /// Adds a new SignalR connection to a group shared between two users.
    /// </summary>
    /// <param name="issuerId">The ID of the connection issuer user.</param>
    /// <param name="otherUserId">The ID of the other user.</param>
    /// <param name="connectionId">The unique SignalR connection identifier.</param>
    /// <returns>
    /// The <see cref="Group"/> that the connection was added to.
    /// If the group did not exist, it will be created.
    /// </returns>
    public Group AddConnection(int issuerId, int otherUserId, string connectionId);

    /// <summary>
    /// Removes a connection from all tracked groups.
    /// </summary>
    /// <param name="connectionId">The SignalR connection identifier to remove.</param>
    /// <returns>
    /// The <see cref="Group"/> from which the connection was removed,
    /// or <see langword="null"/> if no group contained the connection.
    /// </returns>
    public Group? RemoveConnection(string connectionId);
}
