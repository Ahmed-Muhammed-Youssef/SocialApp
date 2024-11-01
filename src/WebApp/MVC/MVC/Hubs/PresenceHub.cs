using Infrastructure.RealTime.Presence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MVC.Hubs
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly OnlinePresenceManager _presenceManager;

        public PresenceHub(OnlinePresenceManager presenceManager)
        {
            _presenceManager = presenceManager;
        }
        public override async Task OnConnectedAsync()
        {
            string identityId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var isFirstConnection = await _presenceManager.UserConnected(identityId, Context.ConnectionId);
            if (isFirstConnection)
            {
                await Clients.Others.SendAsync("UserIsOnline", identityId.ToString());
            }
            var currentUsers = await _presenceManager.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string identityId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var isJustDisconnected = await _presenceManager.UserDisconnected(identityId, Context.ConnectionId);
            if (isJustDisconnected)
            {
                await Clients.Others.SendAsync("UserIsOffline", identityId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
