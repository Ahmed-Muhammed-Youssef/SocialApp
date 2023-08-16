using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFriendRequestsRepository
    {
        public Task<bool> SendFriendRequest(int likerId, int likedId);
        public Task<FriendRequest> GetFriendRequestAsync(int likerId, int likedId);
        public Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int likerId);
        public Task<IEnumerable<int>> GetFriendRequestedUsersIdAsync(int likerId);
        public Task<bool> IsFriend(int userId, int matchedId);
        public Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams);
    }
}
