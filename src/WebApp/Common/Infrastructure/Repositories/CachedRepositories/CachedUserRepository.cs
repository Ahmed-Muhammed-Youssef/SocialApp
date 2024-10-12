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

        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            object key = nameof(GetUserByIdAsync) + id;
            if (!_memoryCache.TryGetValue(key, out ApplicationUser result))
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
        public void DeleteUser(ApplicationUser user) => _usersRepository.DeleteUser(user);

        public void Update(ApplicationUser appUser) => _usersRepository.Update(appUser);

        public Task AddApplicationUser(ApplicationUser user) => _usersRepository.AddApplicationUser(user);
    }
}
