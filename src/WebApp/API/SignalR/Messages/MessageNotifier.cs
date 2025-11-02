using Application.Features.Messages;

namespace API.SignalR.Messages;

public class MessageNotifier (IHubContext<PresenceHub> presenceHubContext, IOnlinePresenceManager presenceTracker) : IMessageNotifier
{
    /// <inheritdoc/>
    public async Task NotifyRecipientAsync(UserDTO sender, MessageDTO message)
    {
        var recipientConnections = await presenceTracker.GetConnectionForUser(message.RecipientId);
        if (recipientConnections != null && recipientConnections.Count != 0)
        {
            await presenceHubContext.Clients.Clients(recipientConnections)
                .SendAsync("NewMessage", new { sender, message });
        }
    }
}
