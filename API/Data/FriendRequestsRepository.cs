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
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public FriendRequestsRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<bool> SendFriendRequest(int senderId, int targetId)
        {
            await _dataContext.FriendRequests.AddAsync(new FriendRequest() { 
                RequesterId = senderId,
                RequestedId = targetId
            });

            // check if the taret id sent him a request already or not if it does, add him to friends

            var frFromTarget = await _dataContext.FriendRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(fr => fr.RequesterId == targetId && fr.RequestedId == senderId);
            if (frFromTarget != null)
            {
                await _dataContext.Friends.AddAsync(new Friend { UserId = senderId, FriendId = targetId });
                await _dataContext.Friends.AddAsync(new Friend { UserId = targetId, FriendId = senderId });
                _dataContext.FriendRequests.Remove(frFromTarget);
            }
            return frFromTarget != null;
        }
        public async Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams)
        {
            var friendsIds = await _dataContext.Friends
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => u.FriendId).ToListAsync();

            var queryDto = _dataContext.Users
                .Where(user => friendsIds.Contains(user.Id))
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider).AsNoTracking();

            queryDto = queryDto.OrderByDescending(u => u.LastActive);

            var pagedResult = await PagedList<UserDTO>
                .CreatePageAsync(queryDto, paginationParams.PageNumber, paginationParams.ItemsPerPage);

            // pagedResult.ForEach(u => u.Pictures.OrderBy(p => p.Order));

            return pagedResult;
        }
        public async Task<bool> IsFriend(int userId, int targetId)
        {
            return await _dataContext.Friends
                .AsNoTracking()
                .AnyAsync(u => u.UserId == userId && u.FriendId == targetId);
        }
        public async Task<FriendRequest> GetFriendRequestAsync(int senderId, int targetId)
        {
            return await _dataContext.FriendRequests
                .AsNoTracking()
                .Where(fr => fr.RequestedId == targetId && fr.RequesterId == senderId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId)
        {
            var users = _dataContext.FriendRequests
                .AsNoTracking()
                .Where(fr => fr.RequesterId == senderId)
                .Select(fr => fr.Requested)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
            return await users.ToListAsync();
        }

        public async Task<IEnumerable<int>> GetFriendsIdsAsync(int senderId)
        {
            var users = _dataContext.Friends
                .AsNoTracking()
                .Where(f => f.UserId == senderId)
                .Select(f => f.FriendId);
            return await users.ToListAsync();
        }
    }
}
