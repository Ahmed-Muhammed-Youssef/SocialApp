using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message, int issuerId);
        Task<Message> GetMessageAsync(int messageId);
        Task<IEnumerable<MessageDTO>> GetAllPagedMessagesDTOForUserAsync(int userId);
        Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId);
        Task<bool> SaveAsync();
    }
}
