﻿using API.Application.DTOs;
using API.Domain.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<char> GetUserInterest(int userId);
        public void Update(AppUser appUser);
        public Task<PagedList<UserDTO>> GetUsersDTOAsync(string username, UserParams userParams, List<int> ForbiddenIds);
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<UserDTO> GetUserDTOByIdAsync(int id);
        public void DeleteUser(AppUser user);
        public Task<bool> UserExistsAsync(int id);
    }
}