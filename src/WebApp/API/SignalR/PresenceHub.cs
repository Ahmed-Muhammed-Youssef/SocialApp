namespace API.SignalR;

[Authorize]
public class PresenceHub(OnlinePresenceManager _presenceTracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        int id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var isFirstConnection = await _presenceTracker.UserConnected(id, Context.ConnectionId);
        if (isFirstConnection)
        {
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetPublicId().ToString());
        }
        var currentUsers = await _presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        int id = Context.User?.GetPublicId() ?? throw new InvalidDataException("Failed to get user Id");

        var isJustDisconnected = await _presenceTracker.UserDisconnected(id, Context.ConnectionId);
        if (isJustDisconnected)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetPublicId().ToString());
        }
        await base.OnDisconnectedAsync(exception);
    }
}