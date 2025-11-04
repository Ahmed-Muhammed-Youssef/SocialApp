using Application.Features.Users;
using Domain.Entities;
using Shared.RepositoryBase;

namespace Application.Common.Interfaces.Repositories;

public interface IFriendRequestRepository : IRepositoryBase<FriendRequest>
{
    /// <summary>
    /// Sends a friend request from one user to another.
    /// If the target user has already sent a friend request to the sender,
    /// both users are added as friends, and the target user's friend request is removed.
    /// Otherwise, a new friend request is created.
    /// </summary>
    /// <param name="senderId">The ID of the user sending the friend request.</param>
    /// <param name="targetId">The ID of the target user receiving the friend request.</param>
    /// <returns>
    /// A boolean value indicating whether the target user had already sent a friend request
    /// to the sender (true if they were already added as friends, false otherwise).
    /// </returns>
    public Task<bool> SendFriendRequest(int senderId, int targetId);

    /// <summary>
    /// Retrieves a friend request sent by a specific user to a target user.
    /// </summary>
    /// <param name="senderId">The ID of the user who sent the friend request.</param>
    /// <param name="targetId">The ID of the target user who received the friend request.</param>
    /// <returns>
    /// A <see cref="FriendRequest"/> object representing the friend request, or null if no request exists.
    /// </returns>
    public Task<FriendRequest?> GetFriendRequestAsync(int senderId, int targetId);


    /// <summary>
    /// Checks if a friend request has been sent from one user to another.
    /// </summary>
    /// <param name="senderId">The ID of the user who sent the friend request.</param>
    /// <param name="targetId">The ID of the user who received the friend request.</param>
    /// <returns>A boolean value indicating whether a friend request has been sent.</returns>
    public Task<bool> IsFriendRequestedAsync(int senderId, int targetId);

    /// <summary>
    /// Retrieves a list of users who have been sent friend requests by a specific user.
    /// </summary>
    /// <param name="senderId">The ID of the user who sent the friend requests.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="UserDTO"/> objects representing the users who have been sent friend requests.
    /// </returns>
    public Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId);

    /// <summary>
    /// Retrieves a list of friend requests received by a specific user.
    /// </summary>
    /// <param name="targetId">The ID of the user who received the friend requests.</param>
    /// <returns>
    /// A list of <see cref="UserDTO"/> objects representing the users who sent friend requests.
    /// </returns>
    public Task<List<UserDTO>> GetRecievedFriendRequestsAsync(int targetId);

    /// <summary>
    /// Determines whether two users are friends.
    /// </summary>
    /// <param name="userId">The ID of the first user.</param>
    /// <param name="targetId">The ID of the second user.</param>
    /// <returns>A boolean value indicating whether the two users are friends.</returns>
    public Task<bool> IsFriend(int userId, int targetId);
}
