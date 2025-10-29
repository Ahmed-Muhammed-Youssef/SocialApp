using Application.Common.Interfaces;
using Application.Common.Mappings;
using Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Shared.Results;

namespace Application.Features.Messages.Send;

public class SendMessageHandler(IUnitOfWork unitOfWork, IMessageNotifier messageNotifier) 
    : ICommandHandler<SendMessageCommand, Result<SendMessageResult>>
{
    public async ValueTask<Result<SendMessageResult>> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var sender = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.SenderId, cancellationToken);

        var recipient = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.RecipientId, cancellationToken);

        if (recipient == null || sender == null)
        {
            return Result<SendMessageResult>.Error("User not found");
        }
        if (sender.Id == recipient.Id)
        {
            return Result<SendMessageResult>.Error("You can't send messages to yourself");
        }
        if (!await unitOfWork.FriendRequestRepository.IsFriend(sender.Id, recipient.Id))
        {
            return Result<SendMessageResult>.Error("You can only send messages to friends");
        }
        var createdMessage = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = command.Content,
            SenderDeleted = false,
            RecipientDeleted = false,
            ReadDate = null,
            Sender = sender
        };

        var groupName = GetGroupName(sender.Id, recipient.Id);
        var group = await unitOfWork.GroupRepository.GetGroupByName(groupName);

        if (group is not null && group.Connections.Any(c => c.UserId == recipient.Id))
        {
            createdMessage.ReadDate = DateTime.UtcNow;
        }
        else
        {
            await messageNotifier.NotifyRecipientAsync(UserMappings.ToDto(sender), MessageMappings.ToDto(createdMessage));
        }

        try
        {
            await unitOfWork.MessageRepository.AddAsync(createdMessage, cancellationToken);

            return Result<SendMessageResult>.Success(new SendMessageResult(MessageMappings.ToDto(createdMessage), groupName));
        }
        catch (Exception)
        {
            return Result<SendMessageResult>.Error("Couldn't Send the message");
        }
    }

    private static string GetGroupName(int callerId, int otherId) => callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";

}
