
using Application.Interfaces.Repositories;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICachedUserRepository UserRepository { get; }
        IPictureRepository PictureRepository { get; }
        IMessageRepository MessageRepository { get; }
        IFriendRequestRepository FriendRequestRepository { get; }
        IPostRepository PostRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}