using Application.DTOs.Message;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class MessageHub(IUnitOfWork _unitOfWork, IMapper _mapper,
        IHubContext<PresenceHub> _presenceHubContext, PresenceTracker _presenceTracker) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserId = int.Parse(httpContext.Request.Query["userId"]);
            var groupName = GetGroupName(httpContext.User.GetId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            var messages = await _unitOfWork.MessageRepository.GetMessagesDTOThreadAsync(Context.User.GetId(), otherUserId);
            if (_unitOfWork.HasChanges()) await _unitOfWork.SaveChangesAsync();
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessages(NewMessageDTO message)
        {
            var sender = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(Context.User.GetId());
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
                throw new HubException("You can't send messages to an unmatch");

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
            var msgDTO = _mapper.Map<MessageDTO>(createdMessage);

            // @ToDo: Add Profile Picture Here
            // msgDTO.SenderPhotoUrl = null;
            var groupName = GetGroupName(sender.Id, recipient.Id);
            var group = await _unitOfWork.MessageRepository.GetGroupByName(groupName);
            if (group.Connections.Any(c => c.UserId == recipient.Id))
            {
                createdMessage.ReadDate = DateTime.UtcNow;
                msgDTO.ReadDate = createdMessage.ReadDate;
            }
            else
            {
                var recipientConnections = await _presenceTracker.GetConnectionForUser(recipient.Id);
                if (recipientConnections != null)
                {
                    var senderDTO = _mapper.Map<UserDTO>(sender);
                    await _presenceHubContext.Clients.Clients(recipientConnections)
                    .SendAsync("NewMessage", new { senderDTO, msgDTO });
                }
            }
            await _unitOfWork.MessageRepository.AddMessageAsync(createdMessage);
            if (await _unitOfWork.SaveChangesAsync())
            {
                msgDTO.Id = createdMessage.Id;
                await Clients.Group(groupName).SendAsync("NewMessage", msgDTO);
            }
            else
            {
                throw new HubException("Couldn't Send the message");
            }
        }

        // utility methods
        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetGroupByName(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetId());

            if (group == null)
            {
                group = new Group(name: groupName);
                await _unitOfWork.MessageRepository.AddGroupAsync(group);
            }
            group.Connections.Add(connection);

            if (await _unitOfWork.SaveChangesAsync())
            {
                return group;
            }
            throw new HubException("Failed to create group");
        }
        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return group;
            }
            throw new HubException("Failed to remove group");
        }
        private string GetGroupName(int callerId, int otherId)
        {
            return callerId > otherId ? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
        }

    }
}