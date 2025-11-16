namespace API.Features.DirectChats;

public interface IMessageNotifier
{
    /// <summary>
    /// Sends a notification to the recipient about a new message.
    /// </summary>
    /// <remarks>This method checks the recipient's connection status and sends the message notification only
    /// if the recipient is currently connected.</remarks>
    /// <param name="sender">The user who sent the message.</param>
    /// <param name="message">The message to be delivered to the recipient.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task NotifyRecipientAsync(UserDTO sender, MessageDTO message, CancellationToken cancellationToken = default);
}
