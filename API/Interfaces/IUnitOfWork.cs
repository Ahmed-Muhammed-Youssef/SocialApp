using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        IPictureRepository PicturesRepository { get; }
        IMessagesRepository MessagesRepository { get; }
        IFriendRequestsRepository FriendRequestsRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}