using Application.DTOs.Pagination;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFriendRequestRepository
    {
        public Task<bool> SendFriendRequest(int senderId, int targetId);
        public Task<FriendRequest> GetFriendRequestAsync(int senderId, int targetId);
        public Task<bool> IsFriendRequestedAsync(int senderId, int targetId);
        public Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId);
        public Task<List<UserDTO>> GetRecievedFriendRequestsAsync(int targetId);
        public Task<bool> IsFriend(int userId, int targetId);
        public Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams);
        public void DeleteFriendRequest(FriendRequest friendRequest);
    }
}
