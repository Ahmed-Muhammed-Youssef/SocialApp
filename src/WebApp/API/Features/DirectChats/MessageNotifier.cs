using Application.Features.DirectChats.Stores;

namespace API.Features.DirectChats;

public class MessageNotifier (IHubContext<OnlineUsersHub> presenceHubContext, IOnlineUsersStore onlineUsersStore) : IMessageNotifier
{
    /// <inheritdoc/>
    public async Task NotifyRecipientAsync(UserDTO sender, MessageDTO message, CancellationToken cancellationToken = default)
    {
        var recipientConnections = await onlineUsersStore.GetConnectionsByUserId(message.RecipientId);
        if (recipientConnections != null && recipientConnections.Count != 0)
        {
            await presenceHubContext.Clients.Clients(recipientConnections)
                .SendAsync("NewMessage", new { sender, message }, cancellationToken: cancellationToken);
        }
    }
}
