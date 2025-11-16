namespace Application.Features.DirectChats.DisconnectFromChat;

public record DisconnectFromChatCommand(string ConnectionId) : ICommand<Result<DisconnectFromChatResult>>;

public record DisconnectFromChatResult(Group? Group);

