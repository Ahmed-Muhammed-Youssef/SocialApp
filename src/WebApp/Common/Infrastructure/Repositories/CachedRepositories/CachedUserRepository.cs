using Microsoft.Extensions.Caching.Memory;
using Domain.Entities;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.DTOs.User;
using Application.DTOs.Pagination;

namespace Infrastructure.Repositories.CachedRepositories
{
    public class CachedUserRepository(IApplicationUserRepository _usersRepository, IMemoryCache _memoryCache) : ICachedApplicationUserRepository
    {
        // Queries

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            object key = nameof(GetByIdAsync) + id;
            if (!_memoryCache.TryGetValue(key, out ApplicationUser result))
            {
                result = await _usersRepository.GetByIdAsync(id);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

        public async Task<UserDTO> GetDtoByIdAsync(int id)
        {
            object key = nameof(GetDtoByIdAsync) + id;
            if (!_memoryCache.TryGetValue(key, out UserDTO result))
            {
                result = await _usersRepository.GetDtoByIdAsync(id);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

        // Caching this mehtod will need a complex implemention
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams) => _usersRepository.GetUsersDTOAsync(userId, userParams);
        public Task<bool> IdExistsAsync(int id) => _usersRepository.IdExistsAsync(id);

        // Commands
        public void Delete(ApplicationUser user) => _usersRepository.Delete(user);

        public void Update(ApplicationUser appUser) => _usersRepository.Update(appUser);

        public Task AddAsync(ApplicationUser user) => _usersRepository.AddAsync(user);

        public Task<UserDTO> GetDtoByIdentityId(string identityId) => _usersRepository.GetDtoByIdentityId(identityId);

        public Task<ApplicationUser> GetByIdentity(string identity) => _usersRepository.GetByIdentity(identity);
    }
}
