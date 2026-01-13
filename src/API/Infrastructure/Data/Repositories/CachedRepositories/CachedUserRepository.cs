namespace Infrastructure.Data.Repositories.CachedRepositories;

public class CachedUserRepository(IApplicationUserRepository usersRepository, IMemoryCache memoryCache, ApplicationDatabaseContext dataContext) : RepositoryBase<ApplicationUser>(dataContext), IApplicationUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        object key = nameof(GetByIdAsync) + id;
        if (!memoryCache.TryGetValue(key, out ApplicationUser? result))
        {
            result = await usersRepository.GetByIdAsync(id, cancellationToken);

            // cache the value
            memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
        }
        return result;
    }

    public async Task<UserDTO?> GetDtoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        object key = nameof(GetDtoByIdAsync) + id;
        if (!memoryCache.TryGetValue(key, out UserDTO? result))
        {
            result = await usersRepository.GetDtoByIdAsync(id, cancellationToken);

            // cache the value
            memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
        }
        return result;
    }

    public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams, CancellationToken cancellationToken = default) => usersRepository.GetUsersDTOAsync(userId, userParams, cancellationToken);

    public Task<int> SetProfilePictureIfOwnedAsync(int userId, int pictureId, CancellationToken cancellationToken = default) => usersRepository.SetProfilePictureIfOwnedAsync(userId, pictureId, cancellationToken);

    public void AddUserPicture(int userId, int pictureId) => usersRepository.AddUserPicture(userId, pictureId);

    public Task<UserDTO?> GetDtoByIdentityAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return usersRepository.GetDtoByIdentityAsync(identityId, cancellationToken);
    }
}
