using Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Application.DTOs.Message;
using Application.DTOs.User;

namespace Infrastructure.Repositories;

public class MessageRepository(DataContext _dataContext, IMapper _mapper) : IMessageRepository
{
    public async Task AddMessageAsync(Message message)
    {
        await _dataContext.Messages.AddAsync(message);
    }

    public void DeleteMessage(Message message, int issuerId)
    {

        // Delete the message when it's deleted from both sender and recipient
        if (message.SenderId == issuerId)
        {
            if (message.SenderDeleted == false)
            {
                message.SenderDeleted = true;
            }
        }
        else if (message.RecipientId == issuerId)
        {
            if (message.RecipientDeleted == false)
            {
                message.RecipientDeleted = true;
            }
        }
        if (message.RecipientDeleted && message.SenderDeleted)
        {
            _dataContext.Messages.Remove(message);
        }
        else
        {
            _dataContext.Messages.Update(message);
        }
    }
    public async Task<Message?> GetMessageAsync(int messageId)
    {
        return await _dataContext.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }
    public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId)
    {
        var query = _dataContext.Messages
            .AsNoTracking()
            .Where(m => m.SenderId == issuerId && m.RecipientId == theOtherUserId && !m.SenderDeleted || m.SenderId == theOtherUserId && m.RecipientId == issuerId && !m.RecipientDeleted)
            .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
            .OrderBy(m => m.SentDate);

        // update unread messages state to read
        var unreadMessages = query.Where(m => m.ReadDate == null && m.RecipientId == issuerId);
        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.ReadDate = DateTime.UtcNow;
            }
        }
        return await query.ToListAsync(); ;
    }

    public async Task AddGroupAsync(Group group)
    {
        await _dataContext.Groups.AddAsync(group);
    }
    public void RemoveConnection(Connection connection)
    {
        _dataContext.Connections.Remove(connection);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _dataContext.Connections
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ConnectionId == connectionId);
    }

    public async Task<Group?> GetGroupByName(string groupName)
    {
        return await _dataContext.Groups
        .Include(g => g.Connections)
        .FirstOrDefaultAsync(g => g.Name == groupName);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await _dataContext.Groups
        .AsNoTracking()
        .Include(g => g.Connections)
        .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
        .FirstOrDefaultAsync();
    }

    public async Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId)
    {
        var friends = await _dataContext.Friends.Where(fr => fr.UserId == userId).Include(f => f.FriendUser).Select(f => new SimplifiedUserDTO
        {
            Id = f.FriendId,
            FirstName = f.FriendUser!.FirstName,
            LastName = f.FriendUser.LastName,
            ProfilePictureUrl =f.FriendUser.ProfilePictureUrl
        }).ToListAsync();

        return friends;
    }
}
