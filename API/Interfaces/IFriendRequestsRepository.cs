using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFriendRequestsRepository
    {
        public Task<bool> SendFriendRequest(int senderId, int targetId);
        public Task<FriendRequest> GetFriendRequestAsync(int senderId, int targetId);
        public Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId);
        public Task<IEnumerable<int>> GetFriendsIdsAsync(int senderId);
        public Task<bool> IsFriend(int userId, int targetId);
        public Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams);
    }
}
