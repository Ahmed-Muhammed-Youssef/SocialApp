using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.CachedRepositories
{
    public class CachedUserRepository : ICachedUserRepository
    {
        private readonly IUserRepository _usersRepository;

        public CachedUserRepository(IUserRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }
        // Queries
        public Task<AppUser> GetUserByEmailAsync(string email) => _usersRepository.GetUserByEmailAsync(email);

        public Task<AppUser> GetUserByIdAsync(int id) => _usersRepository.GetUserByIdAsync(id);

        public Task<AppUser> GetUserByUsernameAsync(string username) => _usersRepository.GetUserByUsernameAsync(username);

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
