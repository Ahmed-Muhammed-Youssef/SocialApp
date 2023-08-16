using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class FriendRequestsRepository : IFriendRequestsRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public FriendRequestsRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<bool> SendFriendRequest(int senderId, int targetId)
        {
            await dataContext.FriendRequests.AddAsync(new FriendRequest() { 
                RequesterId = senderId,
                RequestedId = targetId
            });

            // check if the friend id sent him a request already or not if it does, add him to friends

            bool isFriend = await dataContext.FriendRequests.AnyAsync(fr => fr.RequesterId == targetId
            && fr.RequestedId == senderId);
            if (isFriend)
            {
                await dataContext.Friends.AddAsync(new Friend { UserId = senderId, FriendId = targetId });
                await dataContext.Friends.AddAsync(new Friend { UserId = targetId, FriendId = senderId });
            }
            return isFriend;
        }
        public async Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams)
        {
            var friendsIds = await dataContext.Friends.Where(u => u.UserId == id)
                .Select(u => u.FriendId).ToListAsync();

            var queryDto = dataContext.Users.Where(user => friendsIds.Contains(user.Id))
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).AsNoTracking();

            queryDto = queryDto.OrderByDescending(u => u.LastActive);

            var pagedResult = await PagedList<UserDTO>
                .CreatePageAsync(queryDto, paginationParams.PageNumber, paginationParams.ItemsPerPage);

            pagedResult.ForEach(u => u.Photos.OrderBy(p => p.Order));

            return pagedResult;
        }
        public async Task<bool> IsFriend(int userId, int targetId)
        {
            return await dataContext.Friends
                .AnyAsync(u => u.UserId == userId && u.FriendId == targetId);
        }
        public async Task<FriendRequest> GetFriendRequestAsync(int senderId, int targetId)
        {
            return await dataContext.FriendRequests
                .Where(fr => fr.RequestedId == targetId && fr.RequesterId == senderId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId)
        {
            var users = dataContext.FriendRequests
                .Where(fr => fr.RequesterId == senderId)
                .Select(fr => fr.Requested)
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider);
            return await users.ToListAsync();
        }

        public async Task<IEnumerable<int>> GetFriendRequestedUsersIdAsync(int senderId)
        {
            var users = dataContext.FriendRequests
                .Where(fr => fr.RequesterId == senderId)
                .Select(fr => fr.RequestedId);
            return await users.ToListAsync();
        }
    }
}
