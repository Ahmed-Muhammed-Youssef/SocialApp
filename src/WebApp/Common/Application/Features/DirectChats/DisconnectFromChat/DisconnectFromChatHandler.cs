using Application.Features.DirectChats.Stores;

namespace Application.Features.DirectChats.DisconnectFromChat;

public class DisconnectFromChatHandler(IDirectChatGroupStore usersGroupsStore) : ICommandHandler<DisconnectFromChatCommand, Result<DisconnectFromChatResult>>
{
    public ValueTask<Result<DisconnectFromChatResult>> Handle(DisconnectFromChatCommand command, CancellationToken cancellationToken)
    {
        Group? group = usersGroupsStore.RemoveConnection(command.ConnectionId);

        return ValueTask.FromResult(Result<DisconnectFromChatResult>.Success(new DisconnectFromChatResult(group)));
    }
}
