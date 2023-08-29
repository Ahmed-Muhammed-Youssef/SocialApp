using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessagesRepository
    {
        Task AddGroupAsync(Group group);
        void RemoveConnection(Connection connection);
        Task<Group> GetGroupForConnection(string connectionId);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetGroupByName(string groupName);
        Task AddMessageAsync(Message message);
        void DeleteMessage(Message message, int issuerId);
        Task<Message> GetMessageAsync(int messageId);
        Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId);
    }
}
