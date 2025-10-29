using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Messages.DisconnectFromChat;

public class DisconnectFromChatHandler(IUnitOfWork unitOfWork) : ICommandHandler<DisconnectFromChatCommand, Result<DisconnectFromChatResult>>
{
    public async ValueTask<Result<DisconnectFromChatResult>> Handle(DisconnectFromChatCommand command, CancellationToken cancellationToken)
    {
        Group? group = await unitOfWork.GroupRepository.GetGroupByConnectionId(command.ConnectionId, cancellationToken);

        if (group is not null)
        {
            Connection? connection = group.Connections.FirstOrDefault(c => c.ConnectionId == command.ConnectionId);

            if (connection is not null)
            {
                await unitOfWork.ConnectionRepository.DeleteAsync(connection, cancellationToken);
            }
        }

        return Result<DisconnectFromChatResult>.Success(new DisconnectFromChatResult(group)); ;
    }
}
