using System.Threading.Tasks;
using API.Interfaces.Repositories;

namespace API.Interfaces
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