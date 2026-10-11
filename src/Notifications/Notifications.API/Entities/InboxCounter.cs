namespace Notifications.API.Entities;

/// <summary>
/// The last inbox sequence number handed out to a recipient. Push locks this row to take the next number,
/// so a recipient's numbers become visible in order: a reader never sees seq N+1 before N is committed.
/// </summary>
public sealed class InboxCounter
{
    public required string RecipientId { get; init; }
    public long LastSeq { get; init; }
}
