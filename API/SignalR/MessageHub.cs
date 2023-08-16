using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        private readonly IHubContext<PresenceHub> presenceHubContext;
        private readonly PresenceTracker presenceTracker;

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, 
            IHubContext<PresenceHub> presenceHubContext, PresenceTracker presenceTracker)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.presenceHubContext = presenceHubContext;
            this.presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserId = int.Parse(httpContext.Request.Query["userId"]);
            var groupName = GetGroupName(httpContext.User.GetId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            var messages = await unitOfWork.MessagesRepository.GetMessagesDTOThreadAsync(Context.User.GetId(), otherUserId);
            if(unitOfWork.HasChanges()) await unitOfWork.Complete();
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
            var sender = await unitOfWork.UsersRepository.GetUserByIdAsync(Context.User.GetId());
            var recipient = await unitOfWork.UsersRepository.GetUserByUsernameAsync(message.RecipientUsername);
            if(recipient == null || sender == null)
            {
                throw new HubException("User not found");
            }
            if(sender.Id == recipient.Id)
            {
               throw new HubException("You can't send messages to yourself");
            }
            if (!await unitOfWork.FriendRequestsRepository.IsFriend(sender.Id, recipient.Id))
            {
                throw new HubException("You can't send messages to an unmatch");

            }
            var createdMessage = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Sender = sender,
                Recipient = recipient,
                Content = message.Content,
                SenderDeleted = false,
                RecipientDeleted = false,
                ReadDate = null
            };
            var msgDTO = mapper.Map<MessageDTO>(createdMessage);
            var profilePhoto = await unitOfWork.UsersRepository.GetProfilePictureAsync(sender.Id);
            if(profilePhoto != null)
            {
                msgDTO.SenderPhotoUrl = profilePhoto.Url;
            }
            var groupName = GetGroupName(sender.Id, recipient.Id);
            var group = await unitOfWork.MessagesRepository.GetMessageGroup(groupName);
            if(group.Connections.Any(c => c.UserId == recipient.Id))
            {
                createdMessage.ReadDate = DateTime.UtcNow;
                msgDTO.ReadDate = createdMessage.ReadDate;
            }
            else 
            {
                var recipientConnections = await presenceTracker.GetConnectionForUser(recipient.UserName);
                if(recipientConnections != null)
                {
                    var senderDTO = mapper.Map<UserDTO>(sender);
                    await presenceHubContext.Clients.Clients(recipientConnections)
                    .SendAsync("NewMessage", new { senderDTO, msgDTO });
                }
            }
            unitOfWork.MessagesRepository.AddMessage(createdMessage);
            if (await unitOfWork.Complete())
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
        private async Task<Group> AddToGroup(string groupName){
            var group = await unitOfWork.MessagesRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetId());

            if(group == null)
            {
                group = new Group(name: groupName);
                unitOfWork.MessagesRepository.AddGroup(group);
            }
            group.Connections.Add(connection);
            if(await unitOfWork.Complete())
            {
                return group;
            }
            throw new HubException("Failed to craete group");
        }
        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await unitOfWork.MessagesRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            unitOfWork.MessagesRepository.RemoveConnection(connection);
            if(await unitOfWork.Complete())
            {
                return group;
            }
           throw new HubException("Failed to remove group");
        }
        private string GetGroupName(int callerId, int otherId)
        {
            return callerId > otherId? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
        }

    }
}