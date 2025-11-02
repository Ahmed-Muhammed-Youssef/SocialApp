namespace Infrastructure.Data;

public class UnitOfWork(ApplicationDatabaseContext _dataContext, IApplicationUserRepository userRepository, IPictureRepository pictureRepository,
    IMessageRepository messageRepository, IFriendRequestRepository friendRequestRepository, IPostRepository postRepository,
    IGroupRepository groupRepository, IConnectionRepository connectionRepository) : IUnitOfWork
{
    public IPostRepository PostRepository { get; } = postRepository;
    public IApplicationUserRepository ApplicationUserRepository { get; } = userRepository;
    public IPictureRepository PictureRepository { get; } = pictureRepository;
    public IMessageRepository MessageRepository { get; } = messageRepository;
    public IFriendRequestRepository FriendRequestRepository { get; } = friendRequestRepository;
    public IGroupRepository GroupRepository { get; } = groupRepository;
    public IConnectionRepository ConnectionRepository { get; } = connectionRepository;
    public async Task SaveChangesAsync()
    {
        await _dataContext.SaveChangesAsync();
    }
    public bool HasChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }
}