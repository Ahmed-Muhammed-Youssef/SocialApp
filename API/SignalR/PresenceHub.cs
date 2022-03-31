using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            this.presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            var isFirstConnection = await presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if(isFirstConnection){
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            }
            var currentUsers = await presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isJustDisconnected = await presenceTracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            if(isJustDisconnected){
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}