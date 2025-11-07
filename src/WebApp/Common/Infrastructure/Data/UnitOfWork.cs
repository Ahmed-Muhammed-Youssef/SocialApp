namespace Infrastructure.Data;

public class UnitOfWork(ApplicationDatabaseContext _dataContext, IApplicationUserRepository userRepository, IPictureRepository pictureRepository,
    IDirectChatRepository directChatRepository, IFriendRequestRepository friendRequestRepository, IPostRepository postRepository,
    IGroupRepository groupRepository, IConnectionRepository connectionRepository) : IUnitOfWork
{
    public IPostRepository PostRepository { get; } = postRepository;
    public IApplicationUserRepository ApplicationUserRepository { get; } = userRepository;
    public IPictureRepository PictureRepository { get; } = pictureRepository;
    public IDirectChatRepository DirectChatRepository { get; } = directChatRepository;
    public IFriendRequestRepository FriendRequestRepository { get; } = friendRequestRepository;
    public IGroupRepository GroupRepository { get; } = groupRepository;
    public IConnectionRepository ConnectionRepository { get; } = connectionRepository;

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
    public bool HasChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }
}