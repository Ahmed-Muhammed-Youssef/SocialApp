using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork(DataContext _dataContext, IApplicationUserRepository userRepository, IPictureRepository pictureRepository,
    IMessageRepository messageRepository, IFriendRequestRepository friendRequestRepository, IPostRepository postRepository) : IUnitOfWork
{
    public IPostRepository PostRepository { get; } = postRepository;
    public IApplicationUserRepository ApplicationUserRepository { get; } = userRepository;
    public IPictureRepository PictureRepository { get; } = pictureRepository;
    public IMessageRepository MessageRepository { get; } = messageRepository;
    public IFriendRequestRepository FriendRequestRepository { get; } = friendRequestRepository;
    public async Task SaveChangesAsync()
    {
        await _dataContext.SaveChangesAsync();
    }
    public bool HasChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }
}