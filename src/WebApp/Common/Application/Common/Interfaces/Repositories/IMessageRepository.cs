using Application.Features.Messages;
using Application.Features.Users;
using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface IMessageRepository : IRepositoryBase<Message>
{
    void DeleteMessage(Message message, int issuerId);
    Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId, CancellationToken cancellationToken = default);
    Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId);
}
