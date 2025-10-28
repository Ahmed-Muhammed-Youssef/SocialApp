using Application.Common.Interfaces.Repositories;

namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    ICachedApplicationUserRepository ApplicationUserRepository { get; }
    IPictureRepository PictureRepository { get; }
    IMessageRepository MessageRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IPostRepository PostRepository { get; }
    Task SaveChangesAsync();
    bool HasChanges();
}