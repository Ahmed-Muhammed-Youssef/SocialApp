using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public MessageHub(IMessageRepository messageRepository, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserId = int.Parse(httpContext.Request.Query["userId"]);
            var groupName = GetGroupName(httpContext.User.GetId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var messages = await messageRepository.GetMessagesDTOThreadAsync(Context.User.GetId(), otherUserId);
            await Clients.Group(groupName).SendAsync("ReceiveMessages", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
        private string GetGroupName(int callerId, int otherId){
            return callerId >  otherId? $"{callerId}-{otherId}" : $"{otherId}- {callerId}";
        }

    }
}