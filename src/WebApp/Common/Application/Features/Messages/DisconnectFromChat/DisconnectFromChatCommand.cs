using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Messages.DisconnectFromChat;

public record DisconnectFromChatCommand(string ConnectionId) : ICommand<Result<DisconnectFromChatResult>>;

public record DisconnectFromChatResult(Group? Group);

