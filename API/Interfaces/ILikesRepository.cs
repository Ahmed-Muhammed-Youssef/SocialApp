using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        public Task<bool> LikeAsync(int likerId, int likedId);
        public Task<Like> GetLikeAsync(int likerId, int likedId);
        public Task<IEnumerable<UserDTO>> GetLikedUsersDTOAsync(int likerId);
        public Task<IEnumerable<int>> GetLikedUsersIdAsync(int likerId);
        public Task<bool> SaveAllAsync();
        public Task<List<UserDTO>> GetMatchesAsync(int id);
    }
}
