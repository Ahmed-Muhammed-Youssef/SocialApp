using Application.Common.Interfaces.Repositories;

namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUserRepository { get; }
    IPictureRepository PictureRepository { get; }
    IMessageRepository MessageRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IPostRepository PostRepository { get; }
    IGroupRepository GroupRepository { get; }
    IConnectionRepository ConnectionRepository { get; }
    Task SaveChangesAsync();
    bool HasChanges();
}