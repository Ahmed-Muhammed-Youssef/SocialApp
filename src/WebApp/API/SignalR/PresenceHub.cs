using Application.Features.Messages;

namespace API.SignalR;

[Authorize]
public class PresenceHub(IOnlinePresenceManager presenceTracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        int id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var isFirstConnection = await presenceTracker.UserConnected(id, Context.ConnectionId);
        if (isFirstConnection)
        {
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetPublicId().ToString());
        }
        var currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        int id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var isJustDisconnected = await presenceTracker.UserDisconnected(id, Context.ConnectionId);
        if (isJustDisconnected)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetPublicId().ToString());
        }
        await base.OnDisconnectedAsync(exception);
    }
}