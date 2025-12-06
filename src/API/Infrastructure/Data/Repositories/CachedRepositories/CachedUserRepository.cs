namespace Infrastructure.Data.Repositories.CachedRepositories;

public class CachedUserRepository(IApplicationUserRepository _usersRepository, IMemoryCache _memoryCache, ApplicationDatabaseContext dataContext) : RepositoryBase<ApplicationUser>(dataContext), IApplicationUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        object key = nameof(GetByIdAsync) + id;
        if (!_memoryCache.TryGetValue(key, out ApplicationUser? result))
        {
            result = await _usersRepository.GetByIdAsync(id, cancellationToken);

            // cache the value
            _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
        }
        return result;
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id)
    {
        object key = nameof(GetDtoByIdAsync) + id;
        if (!_memoryCache.TryGetValue(key, out UserDTO? result))
        {
            result = await _usersRepository.GetDtoByIdAsync(id);

            // cache the value
            _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
        }
        return result;
    }

    // Caching this mehtod will need a complex implemention
    public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams) => _usersRepository.GetUsersDTOAsync(userId, userParams);

    public Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId) => _usersRepository.SetProfilePictureIfOwnedAsync(userId, pictureId);
}
