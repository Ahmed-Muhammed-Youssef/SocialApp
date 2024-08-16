using Application.DTOs.Message;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IMessageRepository
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
