namespace Infrastructure.Data;

public class UnitOfWork(ApplicationDatabaseContext _dataContext, IApplicationUserRepository userRepository, IPictureRepository pictureRepository,
    IDirectChatRepository directChatRepository, IFriendRequestRepository friendRequestRepository,
    IPostRepository postRepository, IFriendRepository friendRepository) : IUnitOfWork
{
    public IPostRepository PostRepository { get; } = postRepository;
    public IFriendRepository FriendRepository { get; } = friendRepository;
    public IApplicationUserRepository ApplicationUserRepository { get; } = userRepository;
    public IPictureRepository PictureRepository { get; } = pictureRepository;
    public IDirectChatRepository DirectChatRepository { get; } = directChatRepository;
    public IFriendRequestRepository FriendRequestRepository { get; } = friendRequestRepository;

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