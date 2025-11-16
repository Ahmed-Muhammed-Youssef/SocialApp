namespace Application.Common.Interfaces.Repositories;

public interface IDirectChatRepository : IRepositoryBase<DirectChat>
{
    /// <summary>
    /// Gets an existing direct chat between two users or creates a new one if it doesn't exist.
    /// </summary>
    /// <param name="user1Id">First participant Id</param>
    /// <param name="user2Id">Second participant Id</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An existing direct chat if found or the newly created one.</returns>
    Task<DirectChat> GetOrAddAsync(int user1Id, int user2Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all the mesaages between the two users.
    /// </summary>
    /// <param name="user1Id">The first user's ID</param>
    /// <param name="user2Id">The second user's ID</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all the messages</returns>
    /// <remarks>The order of the IDs doesn't matter.</remarks>
    Task<List<MessageDTO>> GetMessagesDTOThreadAsync(int user1Id, int user2Id, CancellationToken cancellationToken = default);
}
