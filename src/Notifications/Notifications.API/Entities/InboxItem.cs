namespace Notifications.API.Entities;

/// <summary>
/// A notification in one recipient's inbox. <see cref="Seq"/> is gapless and ordered per recipient,
/// so clients sync with "seq greater than the last one I have".
/// </summary>
public sealed class InboxItem
{
    public required string RecipientId { get; init; }
    public long Seq { get; init; }
    public required string EventId { get; init; }

    public Notification? Notification { get; init; }
}
