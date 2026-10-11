namespace Notifications.API.Channels.SignalR;

/// <summary>
/// Clients receive <see cref="ReceiveNotificationMethod"/> and must call <see cref="Acknowledge"/> with its <c>eventId</c>;
/// unacknowledged notifications are re-sent, so clients should ignore an <c>eventId</c> they've already shown.
/// </summary>
[Authorize]
public sealed class NotificationsHub(PresenceTracker presence) : Hub
{
    public const string Route = "hubs/notifications";
    public const string ReceiveNotificationMethod = "ReceiveNotification";

    public override async Task OnConnectedAsync()
    {
        string userId = GetUserId();

        // Notifications parked while the user was offline are replayed as soon as their first connection opens.
        bool firstConnection = presence.Connect(userId);
        try
        {
            if (firstConnection)
            {
                // send nonprocessed/valid notifications that he probably missed!
            }
        }
        catch
        {
            presence.Disconnect(userId);
            throw;
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        presence.Disconnect(GetUserId());
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>Confirms the caller received a notification. Acknowledging an unknown or already-delivered event is a no-op.</summary>
    public async Task Acknowledge(string eventId)
    {
        throw new NotImplementedException();
    }

    private string GetUserId() => Context.UserIdentifier ?? throw new HubException("The access token has no user identifier.");
}
