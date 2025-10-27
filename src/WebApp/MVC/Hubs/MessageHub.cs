using Application.Features.Messages;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

namespace MVC.Hubs;

public class MessageHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;

    public MessageHub(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
    public async Task SendMessages(NewMessageDTO message)
    {
        var id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
        var recipient = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(message.RecipientId);

        if (recipient == null || sender == null)
        {
            throw new HubException("User not found");
        }
        if (sender.Id == recipient.Id)
        {
            throw new HubException("You can't send messages to yourself");
        }
        if (!await _unitOfWork.FriendRequestRepository.IsFriend(sender.Id, recipient.Id))
        {
            throw new HubException("You can send messages to friends only");

        }
        var createdMessage = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = message.Content,
            SenderDeleted = false,
            RecipientDeleted = false,
            ReadDate = null
        };

        await _unitOfWork.MessageRepository.AddMessageAsync(createdMessage);

        try
        {
            await _unitOfWork.SaveChangesAsync();

            MessageDTO msgDTO = new()
            {
                Id = createdMessage.Id,
                SenderId = createdMessage.SenderId,
                RecipientId = createdMessage.RecipientId,
                Content = createdMessage.Content,
                SenderPhotoUrl = sender.ProfilePictureUrl
            };

            await Clients.User(msgDTO.RecipientId.ToString()).SendAsync("NewMessage", msgDTO);
        }
        catch (Exception ex)
        {
            throw new HubException("Couldn't Send the message", ex);
        }
    }
}
