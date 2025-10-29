using Mediator;
using Shared.Results;

namespace Application.Features.Messages.ConnectToChat;

public record ConnectToChatCommand(int CurrentUserId, int OtherUserId, string ConnectionId)
    : ICommand<Result<ConnectToChatResult>>;
