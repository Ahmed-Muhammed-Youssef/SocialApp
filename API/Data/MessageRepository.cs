using API.DTOs;
using API.Entities;
using API.Helpers;
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
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public MessageRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message, int issuerId)
        {
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
                dataContext.Messages.Remove(message);
            }
            else
            {
                dataContext.Messages.Update(message);
            }
        }
        public async Task<Message> GetMessageAsync(int messageId)
        {
            return await dataContext.Messages.FindAsync(messageId);
        }
        public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int issuerId, int theOtherUserId)
        {
            var query = dataContext.Messages
                .Where(
                m =>
                (m.SenderId == issuerId && m.RecipientId == theOtherUserId && !m.SenderDeleted)
                || (m.SenderId == theOtherUserId && m.RecipientId == issuerId && !m.RecipientDeleted))
                .ProjectTo<MessageDTO>(mapper.ConfigurationProvider).OrderBy(m => m.SentDate);
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
            dataContext.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            dataContext.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await dataContext.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
           return await dataContext
           .Groups
           .Include(g => g.Connections)
           .FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await dataContext
            .Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
        }
    }
}
