using Infrastructure.RealTime.Presence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;
using System;
using System.Threading.Tasks;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub(OnlinePresenceManager _presenceTracker) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var isFirstConnection = await _presenceTracker.UserConnected(Context.User.GetPublicId().Value.ToString(), Context.ConnectionId);
            if (isFirstConnection)
            {
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetPublicId().Value.ToString());
            }
            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isJustDisconnected = await _presenceTracker.UserDisconnected(Context.User.GetPublicId().Value.ToString(), Context.ConnectionId);
            if (isJustDisconnected)
            {
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetPublicId().Value.ToString());
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}