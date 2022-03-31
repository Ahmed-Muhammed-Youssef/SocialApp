using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Group> GetGroupForConnection(string connectionId);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        void AddMessage(Message message);
        void DeleteMessage(Message message, int issuerId);
        Task<Message> GetMessageAsync(int messageId);
        Task<PagedList<MessageDTO>> GetAllPagedMessagesDTOForUserAsync(int userId, ReceiveMessagesOptions options, PaginationParams paginationParams);
        Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId);
        Task<bool> SaveAsync();
    }
}
