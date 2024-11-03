using Infrastructure.RealTime.Presence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;

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
            int publicId = Context.User.GetPublicId().Value;

            var isFirstConnection = await _presenceManager.UserConnected(publicId, Context.ConnectionId);
            if (isFirstConnection)
            {
                await Clients.Others.SendAsync("UserIsOnline", publicId.ToString());
            }
            var currentUsers = await _presenceManager.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int publicId = Context.User.GetPublicId().Value;
            var isJustDisconnected = await _presenceManager.UserDisconnected(publicId, Context.ConnectionId);
            if (isJustDisconnected)
            {
                await Clients.Others.SendAsync("UserIsOffline", publicId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
