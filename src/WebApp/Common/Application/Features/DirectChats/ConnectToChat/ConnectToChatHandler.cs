namespace Application.Features.DirectChats.ConnectToChat;

public class ConnectToChatHandler(IUnitOfWork unitOfWork) : ICommandHandler<ConnectToChatCommand, Result<ConnectToChatResult>>
{
    public async ValueTask<Result<ConnectToChatResult>> Handle(ConnectToChatCommand command, CancellationToken cancellationToken)
    {
        if (command.CurrentUserId == command.OtherUserId)
        {
            return Result<ConnectToChatResult>.Error("Cannot start a chat with yourself.");
        }

        string groupName = ChatGroupHelper.GetGroupName(command.CurrentUserId, command.OtherUserId);
        Group? group = await unitOfWork.GroupRepository.GetGroupByName(groupName, cancellationToken);
        Connection connection = new(command.ConnectionId, command.CurrentUserId);

        if (group == null)
        {
            group = new Group(name: groupName);
            group.Connections.Add(connection);
            await unitOfWork.GroupRepository.AddAsync(group, cancellationToken);
        }

        IEnumerable<MessageDTO> messages = await unitOfWork.DirectChatRepository
            .GetMessagesDTOThreadAsync(command.CurrentUserId, command.OtherUserId, cancellationToken);
        
        return Result<ConnectToChatResult>.Success(new ConnectToChatResult(group, messages));
    }
}
