using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub(PresenceTracker _presenceTracker) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var isFirstConnection = await _presenceTracker.UserConnected(Context.User.GetId(), Context.ConnectionId);
            if (isFirstConnection)
            {
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetId().ToString());
            }
            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isJustDisconnected = await _presenceTracker.UserDisconnected(Context.User.GetId(), Context.ConnectionId);
            if (isJustDisconnected)
            {
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetId().ToString());
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}