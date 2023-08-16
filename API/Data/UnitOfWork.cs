using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        public UnitOfWork(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }
        public IUsersRepository UsersRepository => new UserRepository(dataContext, mapper);

        public IMessagesRepository MessagesRepository => new MessageRepository(dataContext, mapper);

        public IFriendRequestsRepository FriendRequestsRepository => new FriendRequestsRepository(dataContext, mapper);

        public async Task<bool> Complete()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return dataContext.ChangeTracker.HasChanges();
        }
    }
}