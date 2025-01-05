using Application.Interfaces;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork(DataContext _dataContext, ICachedApplicationUserRepository _userRepository, IPictureRepository _pictureRepository,
        IMessageRepository _messageRepository, IFriendRequestRepository _friendRequestRepository, IPostRepository _postRepository) : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        public IPostRepository PostRepository { get; } = _postRepository;
        public ICachedApplicationUserRepository ApplicationUserRepository { get; } = _userRepository;
        public IPictureRepository PictureRepository { get; } = _pictureRepository;
        public IMessageRepository MessageRepository { get; } = _messageRepository;
        public IFriendRequestRepository FriendRequestRepository { get; } = _friendRequestRepository;
        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
        public async Task CommitAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }
        public bool HasChanges()
        {
            return _dataContext.ChangeTracker.HasChanges();
        }
        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _dataContext.Dispose();
        }
    }
}