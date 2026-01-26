namespace Infrastructure.Data;

public class UnitOfWork(ApplicationDatabaseContext _dataContext, IApplicationUserRepository userRepository, IPictureRepository pictureRepository,
    IDirectChatRepository directChatRepository, IFriendRequestRepository friendRequestRepository,
    IFriendRepository friendRepository, ICityRepository cityRepository) : IUnitOfWork
{
    public IFriendRepository FriendRepository { get; } = friendRepository;
    public IApplicationUserRepository ApplicationUserRepository { get; } = userRepository;
    public IPictureRepository PictureRepository { get; } = pictureRepository;
    public IDirectChatRepository DirectChatRepository { get; } = directChatRepository;
    public IFriendRequestRepository FriendRequestRepository { get; } = friendRequestRepository;
    public ICityRepository CityRepository { get; } = cityRepository;

    /// <inheritdoc/>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataContext.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dataContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public bool HasChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dataContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}
