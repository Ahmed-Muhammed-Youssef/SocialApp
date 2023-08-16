using System;
using System.Collections.Generic;
using System.Linq;
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