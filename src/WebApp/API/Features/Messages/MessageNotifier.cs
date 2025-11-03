namespace API.Features.Messages;

public class MessageNotifier (IHubContext<OnlineUsersHub> presenceHubContext, IOnlineUsersStore presenceTracker) : IMessageNotifier
{
    /// <inheritdoc/>
    public async Task NotifyRecipientAsync(UserDTO sender, MessageDTO message)
    {
        var recipientConnections = await presenceTracker.GetConnectionsByUserId(message.RecipientId);
        if (recipientConnections != null && recipientConnections.Count != 0)
        {
            await presenceHubContext.Clients.Clients(recipientConnections)
                .SendAsync("NewMessage", new { sender, message });
        }
    }
}
