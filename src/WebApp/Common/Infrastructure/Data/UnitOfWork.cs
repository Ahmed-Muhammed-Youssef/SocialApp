using Application.Interfaces;
using Application.Interfaces.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork(DataContext _dataContext, ICachedApplicationUserRepository _userRepository, IPictureRepository _pictureRepository,
        IMessageRepository _messageRepository, IFriendRequestRepository _friendRequestRepository, IPostRepository _postRepository) : IUnitOfWork
    {
        public IPostRepository PostRepository { get; } = _postRepository;
        public ICachedApplicationUserRepository ApplicationUserRepository { get; } = _userRepository;
        public IPictureRepository PictureRepository { get; } = _pictureRepository;
        public IMessageRepository MessageRepository { get; } = _messageRepository;
        public IFriendRequestRepository FriendRequestRepository { get; } = _friendRequestRepository;

        public async Task<bool> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _dataContext.ChangeTracker.HasChanges();
        }
    }
}