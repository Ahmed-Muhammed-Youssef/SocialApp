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
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly ILikesRepository likesRepository;

        public MessageHub(IMessageRepository messageRepository, IMapper mapper,
            IUserRepository userRepository, ILikesRepository likesRepository)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.likesRepository = likesRepository;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserId = int.Parse(httpContext.Request.Query["userId"]);
            var groupName = GetGroupName(httpContext.User.GetId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(groupName);

            var messages = await messageRepository.GetMessagesDTOThreadAsync(Context.User.GetId(), otherUserId);
            await Clients.Group(groupName).SendAsync("ReceiveMessages", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveFromMessageGroup();
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessages(NewMessageDTO message){
            var sender = await userRepository.GetUserByIdAsync(Context.User.GetId());
            var recipient = await userRepository.GetUserByUsernameAsync(message.RecipientUsername);
            if(recipient == null || sender == null)
            {
                throw new HubException("User not found");
            }
            if(sender.Id == recipient.Id)
            {
               throw new HubException("You can't send messages to yourself");
            }
            if (!await likesRepository.IsMacth(sender.Id, recipient.Id))
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
            var groupName = GetGroupName(sender.Id, recipient.Id);
            var group = await messageRepository.GetMessageGroup(groupName);
            if(group.Connections.Any(c => c.UserId == recipient.Id)){
                createdMessage.ReadDate = DateTime.UtcNow;
            }

            messageRepository.AddMessage(createdMessage);
            var msgDTO = mapper.Map<MessageDTO>(createdMessage);
            var profilePhoto = await userRepository.GetProfilePhotoAsync(sender.Id);
            if(profilePhoto != null)
            {
                msgDTO.SenderPhotoUrl = profilePhoto.Url;
            }

           
            if (await messageRepository.SaveAsync())
            {
                msgDTO.Id = createdMessage.Id;
                await Clients.Group(groupName).SendAsync("NewMessage", msgDTO);
            }
            else{
                throw new HubException("Couldn't Send the message"); 
            }
        } 
        // utility methods
        private async Task<bool> AddToGroup(string groupName){
            var group = await messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetId());

            if(group == null){
                group = new Group(name: groupName);
                messageRepository.AddGroup(group);
            }
            group.Connections.Add(connection);

            return await messageRepository.SaveAsync();
        }
        private async Task RemoveFromMessageGroup(){
            var connection = await messageRepository.GetConnection(Context.ConnectionId);
            messageRepository.RemoveConnection(connection);
            await messageRepository.SaveAsync();
        }
        private string GetGroupName(int callerId, int otherId){
            return callerId > otherId? $"{callerId}-{otherId}" : $"{otherId}-{callerId}";
        }

    }
}