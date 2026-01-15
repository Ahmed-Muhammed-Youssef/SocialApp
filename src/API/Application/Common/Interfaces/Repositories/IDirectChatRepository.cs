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

    /// <summary>
    /// Asynchronously retrieves a paginated list of direct chat data transfer objects (DTOs) for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose direct chats are to be retrieved.</param>
    /// <param name="paginationParams">The pagination parameters that define the page size and number for the results.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paged list of direct chat DTOs for
    /// the specified user.</returns>
    Task<PagedList<DirectChatDTO>> GetChatsDtoAsync(int userId, PaginationParams paginationParams, CancellationToken cancellationToken = default);
}
