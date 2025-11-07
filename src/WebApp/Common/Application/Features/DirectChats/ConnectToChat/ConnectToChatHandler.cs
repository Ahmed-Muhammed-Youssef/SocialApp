namespace Application.Features.DirectChats.ConnectToChat;

public class ConnectToChatHandler(IUnitOfWork unitOfWork, IDirectChatGroupsStore usersGroupsStore) : ICommandHandler<ConnectToChatCommand, Result<ConnectToChatResult>>
{
    public async ValueTask<Result<ConnectToChatResult>> Handle(ConnectToChatCommand command, CancellationToken cancellationToken)
    {
        if (command.CurrentUserId == command.OtherUserId)
        {
            return Result<ConnectToChatResult>.Error("Cannot start a chat with yourself.");
        }
        
        Group? group = usersGroupsStore.AddConnection(command.CurrentUserId, command.OtherUserId, command.ConnectionId);

        IEnumerable<MessageDTO> messages = await unitOfWork.DirectChatRepository
            .GetMessagesDTOThreadAsync(command.CurrentUserId, command.OtherUserId, cancellationToken);
        
        return Result<ConnectToChatResult>.Success(new ConnectToChatResult(group, messages));
    }
}
