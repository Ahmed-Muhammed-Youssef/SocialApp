using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        public Task<bool> LikeAsync(int likerId, int likedId);
        public Task<FriendRequest> GetLikeAsync(int likerId, int likedId);
        public Task<IEnumerable<UserDTO>> GetLikedUsersDTOAsync(int likerId);
        public Task<IEnumerable<int>> GetLikedUsersIdAsync(int likerId);
        public Task<bool> IsMacth(int userId, int matchedId);
        public Task<PagedList<UserDTO>> GetMatchesAsync(int id, PaginationParams paginationParams);
    }
}
