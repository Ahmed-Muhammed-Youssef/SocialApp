namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUserRepository { get; }
    IPictureRepository PictureRepository { get; }
    IDirectChatRepository DirectChatRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IPostRepository PostRepository { get; }
    IFriendRepository FriendRepository { get; }
    ICityRepository CityRepository { get; }

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation of beginning a transaction.</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the commit operation.</param>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously rolls back the current database transaction, undoing all changes made during the transaction.
    /// </summary>
    /// <remarks>If no transaction is active, the method completes without performing any action. This method
    /// should be called to revert changes when an error occurs or when the transaction cannot be committed.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the rollback operation.</param>
    /// <returns>A task that represents the asynchronous rollback operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously commits all changes made in the current context to the underlying data store.
    /// </summary>
    /// <remarks>If the context has no pending changes, the method completes successfully and returns 0. This
    /// method is typically used to persist changes in a unit of work pattern. Multiple concurrent calls to this method
    /// are not supported.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the commit operation.</param>
    /// <returns>A task that represents the asynchronous commit operation. The task result contains the number of state entries
    /// written to the data store.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    bool HasChanges();
}