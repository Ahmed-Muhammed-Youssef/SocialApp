
using API.Application.Interfaces.Repositories;

namespace API.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICachedUserRepository UserRepository { get; }
        IPictureRepository PictureRepository { get; }
        IMessageRepository MessageRepository { get; }
        IFriendRequestRepository FriendRequestRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}