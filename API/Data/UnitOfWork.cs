using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using System.Threading.Tasks;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }
        public IUsersRepository UsersRepository => new UserRepository(_dataContext, _mapper);

        public IPictureRepository PicturesRepository => new PictureRepository(_dataContext, _mapper);

        public IMessagesRepository MessagesRepository => new MessageRepository(_dataContext, _mapper);

        public IFriendRequestsRepository FriendRequestsRepository => new FriendRequestsRepository(_dataContext, _mapper);

        public async Task<bool> Complete()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _dataContext.ChangeTracker.HasChanges();
        }
    }
}