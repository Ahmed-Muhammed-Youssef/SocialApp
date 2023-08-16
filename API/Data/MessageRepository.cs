using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            _dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message, int issuerId)
        {

            // Delete the message when it's deleted from both sender and recipient
            if(message.SenderId == issuerId)
            {
                if(message.SenderDeleted == false)
                {
                    message.SenderDeleted = true;
                }
            }
            else if(message.RecipientId == issuerId)
            {
                if (message.RecipientDeleted == false)
                {
                    message.RecipientDeleted = true;
                }
            }
            if(message.RecipientDeleted && message.SenderDeleted)
            {
                _dataContext.Messages.Remove(message);
            }
            else
            {
                _dataContext.Messages.Update(message);
            }
        }
        public async Task<Message> GetMessageAsync(int messageId)
        {
            return await _dataContext.Messages.FindAsync(messageId);
        }
        public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId)
        {
            var query = _dataContext.Messages
                .Where(m => (m.SenderId == issuerId && m.RecipientId == theOtherUserId && !m.SenderDeleted) || (m.SenderId == theOtherUserId && m.RecipientId == issuerId && !m.RecipientDeleted))
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
            return await query.ToListAsync();;
        }

        public void AddGroup(Group group)
        {
            _dataContext.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            _dataContext.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _dataContext.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
           return await _dataContext
           .Groups
           .Include(g => g.Connections)
           .FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _dataContext
            .Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
        }
    }
}
