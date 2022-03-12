using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
                if (message.SenderDeleted == false)
                {
                    message.SenderDeleted = true;
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
        public async Task<bool> SaveAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<MessageDTO>> GetAllPagedMessagesDTOForUserAsync(int userId)
        {
            var query = dataContext.Messages
               .Where(m => m.SenderId == userId && m.RecipientId == userId)
               .ProjectTo<MessageDTO>(mapper.ConfigurationProvider)
               .OrderByDescending(m => m.SentDate);
            return await query.ToListAsync();
        }


        public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId)
        {
            var query = dataContext.Messages
                .Where(m => m.SenderId == senderId && m.RecipientId == recipientId)
                .ProjectTo<MessageDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(m => m.SentDate);
            return await query.ToListAsync();
        }

    }
}
