using Application.Common.Interfaces.Repositories;

namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUserRepository { get; }
    IPictureRepository PictureRepository { get; }
    IDirectChatRepository DirectChatRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IPostRepository PostRepository { get; }
    IFriendRepository FriendRepository { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    bool HasChanges();
}