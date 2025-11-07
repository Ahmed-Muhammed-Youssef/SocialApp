namespace Application.Features.DirectChats;

/// <summary>
/// Manages user connections and online presence within a system.
/// </summary>
/// <remarks>This interface provides methods to track user connections, determine online status, and retrieve
/// connection details. It is designed to handle multiple connections per user and efficiently manage their online
/// presence.</remarks>
public interface IOnlineUsersStore
{
    /// <summary>
    /// Adds a connection for a user. If this is the first connection for the user, returns true.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="connectionId">The unique connection ID for this session.</param>
    /// <returns>True if this is the user's first connection; otherwise, false.</returns>
    public Task<bool> AddUserConnection(int userId, string connectionId);

    /// <summary>
    /// Removes a connection for a user. If this was the user's last connection, returns true.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="connectionId">The unique connection ID for this session.</param>
    /// <returns>True if the user has no remaining connections; otherwise, false.</returns>
    public Task<bool> RemoveUserConnection(int userId, string connectionId);

    /// <summary>
    /// Retrieves a list of currently online users by their user IDs.
    /// </summary>
    /// <returns>An array of user IDs for online users, sorted in ascending order.</returns>
    public Task<int[]> GetOnlineUsers();

    /// <summary>
    /// Retrieves all connection IDs associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    public Task<IReadOnlyList<string>> GetConnectionsByUserId(int userId);
}
