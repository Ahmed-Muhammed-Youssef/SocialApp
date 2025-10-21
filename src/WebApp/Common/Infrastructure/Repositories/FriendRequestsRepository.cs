using Domain.Entities;
using Infrastructure.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Application.DTOs.User;
using Application.DTOs.Pagination;

namespace Infrastructure.Repositories;

public class FriendRequestsRepository(DataContext _dataContext, IMapper _mapper) : IFriendRequestRepository
{
    public async Task<bool> SendFriendRequest(int senderId, int targetId)
    {
        var frFromTarget = await _dataContext.FriendRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(fr => fr.RequesterId == targetId && fr.RequestedId == senderId);
        if (frFromTarget != null)
        {
            await _dataContext.Friends.AddAsync(new Friend { UserId = senderId, FriendId = targetId });
            await _dataContext.Friends.AddAsync(new Friend { UserId = targetId, FriendId = senderId });
            _dataContext.FriendRequests.Remove(frFromTarget);
        }
        else
        {
            await _dataContext.FriendRequests.AddAsync(new FriendRequest()
            {
                RequesterId = senderId,
                RequestedId = targetId
            });
        }
        return frFromTarget != null;
    }
    public async Task<PagedList<UserDTO>> GetFriendsAsync(int id, PaginationParams paginationParams)
    {
        var friendsIds = await _dataContext.Friends
            .AsNoTracking()
            .Where(u => u.UserId == id)
            .Select(u => u.FriendId).ToListAsync();

        var queryDto = _dataContext.ApplicationUsers
            .Where(user => friendsIds.Contains(user.Id))
            .ProjectTo<UserDTO>(_mapper.ConfigurationProvider).AsNoTracking();

        queryDto = queryDto.OrderByDescending(u => u.LastActive);

        int count = queryDto.Count();
        var items = queryDto.Skip(paginationParams.SkipValue).Take(paginationParams.ItemsPerPage);
        var listDto = await items.ToListAsync();
        var pagedResult = new PagedList<UserDTO>(listDto, listDto.Count, paginationParams.PageNumber, paginationParams.ItemsPerPage);
        return pagedResult;
    }
    public async Task<bool> IsFriend(int userId, int targetId)
    {
        return await _dataContext.Friends
            .AsNoTracking()
            .AnyAsync(u => u.UserId == userId && u.FriendId == targetId);
    }
    public async Task<FriendRequest?> GetFriendRequestAsync(int senderId, int targetId)
    {
        return await _dataContext.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.RequestedId == targetId && fr.RequesterId == senderId)
            .FirstOrDefaultAsync();
    }
    public async Task<bool> IsFriendRequestedAsync(int senderId, int targetId)
    {
        return await _dataContext.FriendRequests
            .AsNoTracking()
            .AnyAsync(fr => fr.RequestedId == targetId && fr.RequesterId == senderId);
    }
    public async Task<List<UserDTO>> GetRecievedFriendRequestsAsync(int targetId)
    {
        return await _dataContext.FriendRequests
            .AsNoTracking()
            .Include(fr => fr.Requester)
            .Where(fr => fr.RequestedId == targetId)
            .Select(fr => fr.Requester)
            .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
    public async Task<IEnumerable<UserDTO>> GetFriendRequestedUsersDTOAsync(int senderId)
    {
        IEnumerable<UserDTO> users = [];

        using (var connection = new SqlConnection(_dataContext.Database.GetDbConnection().ConnectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@senderId", senderId);

            users = await connection.QueryAsync<UserDTO>("GetFriendRequestedUsersDTO", parameters, commandType: CommandType.StoredProcedure);
        }
        return users;
    }
    public void Delete(FriendRequest friendRequest)
    {
        _dataContext.FriendRequests.Entry(friendRequest).State = EntityState.Deleted;
    }
}
