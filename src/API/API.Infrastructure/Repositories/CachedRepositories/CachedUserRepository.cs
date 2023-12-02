using API.Application.DTOs;
using Microsoft.Extensions.Caching.Memory;
using API.Domain.Entities;
using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;

namespace API.Infrastructure.Repositories.CachedRepositories
{
    public class CachedUserRepository : ICachedUserRepository
    {
        private readonly IUserRepository _usersRepository;
        private readonly IMemoryCache _memoryCache;

        public CachedUserRepository(IUserRepository usersRepository, IMemoryCache memoryCache)
        {
            _usersRepository = usersRepository;
            this._memoryCache = memoryCache;
        }
        // Queries

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            object key = nameof(GetUserByIdAsync) + id;
            if (!_memoryCache.TryGetValue(key, out AppUser result))
            {
                result = await _usersRepository.GetUserByIdAsync(id);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            object key = nameof(GetUserDTOByIdAsync) + id;
            if (!_memoryCache.TryGetValue(key, out UserDTO result))
            {
                result = await _usersRepository.GetUserDTOByIdAsync(id);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

        // Caching this mehtod will need a complex implemention
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams) => _usersRepository.GetUsersDTOAsync(userId, userParams);
        public Task<bool> UserExistsAsync(int id) => _usersRepository.UserExistsAsync(id);

        // Commands
        public void DeleteUser(AppUser user) => _usersRepository.DeleteUser(user);

        public void Update(AppUser appUser) => _usersRepository.Update(appUser);
    }
}
