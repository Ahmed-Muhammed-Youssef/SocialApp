using Application.Features.Messages;
using Application.Features.Users;
using Domain.ChatAggregate;
using Shared.RepositoryBase;

namespace Application.Common.Interfaces.Repositories;

public interface IMessageRepository : IRepositoryBase<Message>
{
    Task<IEnumerable<MessageDTO>> GetMessagesDTOThreadAsync(int senderId, int recipientId, CancellationToken cancellationToken = default);
    Task<List<SimplifiedUserDTO>> GetInboxAsync(int userId);
}
