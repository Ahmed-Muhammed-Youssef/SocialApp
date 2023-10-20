using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;

namespace API.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        public UnitOfWork(DataContext dataContext, ICachedUserRepository userRepository, IPictureRepository pictureRepository,
            IMessageRepository messageRepository, IFriendRequestRepository friendRequestRepository)
        {
            _dataContext = dataContext;
            UserRepository = userRepository;
            PictureRepository = pictureRepository;
            MessageRepository = messageRepository;
            FriendRequestRepository = friendRequestRepository;
        }
        public ICachedUserRepository UserRepository { get; }
        public IPictureRepository PictureRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }

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