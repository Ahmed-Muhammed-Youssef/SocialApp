namespace Application.Features.DirectChats.SendMessage;

public class SendMessageHandler(IUnitOfWork unitOfWork, IDirectChatGroupsStore usersGroupsStore)
    : ICommandHandler<SendMessageCommand, Result<SendMessageResult>>
{
    public async ValueTask<Result<SendMessageResult>> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? sender = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.SenderId, cancellationToken);

            ApplicationUser? recipient = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.RecipientId, cancellationToken);

            if (recipient == null || sender == null)
            {
                return Result<SendMessageResult>.Error("User not found");
            }
            if (sender.Id == recipient.Id)
            {
                return Result<SendMessageResult>.Error("You can't send messages to yourself");
            }

            DirectChat chat = await unitOfWork.DirectChatRepository.GetOrAddAsync(sender.Id, recipient.Id, cancellationToken);

            Message newMessage = chat.AddMessage(sender.Id, command.Content);

            await unitOfWork.CommitAsync(cancellationToken);

            Group group = usersGroupsStore.GetOrAddGroup(sender.Id, recipient.Id);

            return Result<SendMessageResult>.Success(new SendMessageResult(MessageMappings.ToDto(newMessage), UserMappings.ToDto(sender), group.Name));
        }
        catch (Exception)
        {
            return Result<SendMessageResult>.Error("Couldn't Send the message");
        }
    }
}
