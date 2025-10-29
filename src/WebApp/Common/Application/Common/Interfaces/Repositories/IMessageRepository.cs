using Application.Features.Messages;
using Application.Features.Users;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IMessageRepository : IRepositoryBase<Message>
{
    Task AddGroupAsync(Group group);
    void RemoveConnection(Connection connection);
    Task<Group?> GetGroupForConnection(string connectionId);
    Task<Connection?> GetConnection(string connectionId);
    Task<Group?> GetGroupByName(string groupName);
    void DeleteMessage(Message message, int issuerId);
    Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId);
    Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId);
}
