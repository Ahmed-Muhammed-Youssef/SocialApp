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
        public async Task<bool> SaveAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<MessageDTO>> GetAllPagedMessagesDTOForUserAsync(int issuerId, ReceiveMessagesOptions options, PaginationParams paginationParams)
        {
            var query = dataContext.Messages
               .OrderByDescending(m => m.SentDate).AsQueryable();
            switch (options)
            {
                case ReceiveMessagesOptions.AllMessages:
                    query = query.Where(m => (m.SenderId == issuerId && !m.SenderDeleted) || (m.RecipientId == issuerId && !m.RecipientDeleted));
                    break;
                case ReceiveMessagesOptions.UnreadMessages:
                    query = query.Where(m => m.RecipientId == issuerId && m.ReadDate == null);
                    break;
                case ReceiveMessagesOptions.SentMessages:
                    query = query.Where(m => m.SenderId == issuerId && !m.SenderDeleted);
                    break;
                case ReceiveMessagesOptions.ReceivedMessages:
                    query = query.Where(m => m.RecipientId == issuerId && !m.RecipientDeleted);
                    break;
            }
            var pagedResult = await PagedList<MessageDTO>.CreatePageAsync(query.ProjectTo<MessageDTO>(mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.ItemsPerPage);
            return pagedResult;
        }


        public async Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId)
        {
            var query = dataContext.Messages
                .Where(
                m =>
                (m.SenderId == senderId && m.RecipientId == recipientId && !m.SenderDeleted)
                || (m.SenderId == recipientId && m.RecipientId == senderId && !m.RecipientDeleted))
                .ProjectTo<MessageDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(m => m.SentDate);

            return await query.ToListAsync();
        }

    }
}
