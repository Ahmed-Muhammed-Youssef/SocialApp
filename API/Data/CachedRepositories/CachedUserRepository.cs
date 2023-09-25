using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace API.Data.CachedRepositories
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
        public async Task<AppUser> GetUserByEmailAsync(string email) 
        {
            object key = nameof(GetUserByEmailAsync) + email;
            if (!_memoryCache.TryGetValue(key, out AppUser result))
            {
                result = await _usersRepository.GetUserByEmailAsync(email);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

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

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            object key = nameof(GetUserByUsernameAsync) + username;
            if (!_memoryCache.TryGetValue(key, out AppUser result))
            {
                result = await _usersRepository.GetUserByUsernameAsync(username);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }

        public Task<UserDTO> GetUserDTOByEmailAsync(string email) => _usersRepository.GetUserDTOByEmailAsync(email);

        public Task<UserDTO> GetUserDTOByIdAsync(int id) => _usersRepository.GetUserDTOByIdAsync(id);

        public Task<UserDTO> GetUserDTOByUsernameAsync(string username) => _usersRepository.GetUserDTOByUsernameAsync(username);

        public Task<char> GetUserInterest(int userId) => _usersRepository.GetUserInterest(userId);

        public Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> ForbiddenIds) =>
            _usersRepository.GetUsersDTOAsync(username, userParams, ForbiddenIds);

        public Task<bool> UserExistsAsync(int id) => _usersRepository.UserExistsAsync(id);

        // Commands
        public void DeleteUser(AppUser user) => _usersRepository.DeleteUser(user);

        public void Update(AppUser appUser) => _usersRepository.Update(appUser);
    }
}
