using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository UsersRepository {get;}
        IMessagesRepository MessagesRepository {get;}
        IFriendRequestsRepository FriendRequestsRepository {get;}
        Task<bool> Complete();
        bool HasChanges();
    }
}