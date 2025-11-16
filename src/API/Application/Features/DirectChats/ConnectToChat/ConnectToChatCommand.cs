namespace Application.Features.DirectChats.ConnectToChat;

public record ConnectToChatCommand(int CurrentUserId, int OtherUserId, string ConnectionId)
    : ICommand<Result<ConnectToChatResult>>;
