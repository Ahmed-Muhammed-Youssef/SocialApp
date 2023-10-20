using API.Application.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;
using API.Domain.Entities;

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

        public async Task<char> GetUserInterest(int userId)
        {
            object key = nameof(GetUserInterest) + userId;
            if (!_memoryCache.TryGetValue(key, out char result))
            {
                result = await _usersRepository.GetUserInterest(userId);

                // cache the value
                _memoryCache.Set(key, result, TimeSpan.FromMinutes(1));
            }
            return result;
        }
        

        // Caching this mehtod will need a complex implemention
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> ForbiddenIds) =>
            _usersRepository.GetUsersDTOAsync(username, userParams, ForbiddenIds);

        public Task<bool> UserExistsAsync(int id) => _usersRepository.UserExistsAsync(id);

        // Commands
        public void DeleteUser(AppUser user) => _usersRepository.DeleteUser(user);

        public void Update(AppUser appUser) => _usersRepository.Update(appUser);
    }
}
