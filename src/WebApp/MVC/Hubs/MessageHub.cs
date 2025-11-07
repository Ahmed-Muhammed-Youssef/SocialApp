using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Features.Messages;
using Domain.ChatAggregate;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

namespace MVC.Hubs;

public class MessageHub(IUnitOfWork unitOfWork) : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
    public async Task SendMessages(NewMessageDTO message)
    {
        var cancellationToken = Context.ConnectionAborted;

        var id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var sender = await unitOfWork.ApplicationUserRepository.GetByIdAsync(id, cancellationToken);
        var recipient = await unitOfWork.ApplicationUserRepository.GetByIdAsync(message.RecipientId, cancellationToken);

        if (recipient == null || sender == null)
        {
            throw new HubException("User not found");
        }
        if (sender.Id == recipient.Id)
        {
            throw new HubException("You can't send messages to yourself");
        }
        if (!await unitOfWork.FriendRequestRepository.IsFriend(sender.Id, recipient.Id))
        {
            throw new HubException("You can send messages to friends only");

        }
        Message createdMessage = new(sender.Id, message.Content);

        await unitOfWork.MessageRepository.AddAsync(createdMessage, cancellationToken);

        try
        {
            await unitOfWork.SaveChangesAsync();
            MessageDTO msgDTO = MessageMappings.ToDto(createdMessage);

            await Clients.User(msgDTO.RecipientId.ToString()).SendAsync("NewMessage", msgDTO);
        }
        catch (Exception ex)
        {
            throw new HubException("Couldn't Send the message", ex);
        }
    }
}
