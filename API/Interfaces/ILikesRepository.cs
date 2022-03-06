using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        public Task<bool> LikeAsync(int likerId, int likedId);
        public Task<bool> SaveAllAsync();
        public Task<List<UserDTO>> GetMatchesAsync(int id);
    }
}
