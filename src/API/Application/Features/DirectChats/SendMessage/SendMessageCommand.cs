namespace Application.Features.DirectChats.SendMessage;

public record SendMessageCommand(int SenderId, int RecipientId, string Content) : ICommand<Result<SendMessageResult>>;
