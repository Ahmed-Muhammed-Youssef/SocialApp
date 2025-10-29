using Mediator;
using Shared.Results;

namespace Application.Features.Messages.Send;

public record SendMessageCommand(int SenderId, int RecipientId, string Content) : ICommand<Result<SendMessageResult>>;
