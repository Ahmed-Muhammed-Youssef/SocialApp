namespace Notifications.API.Entities;

/// <summary>
/// One recipient on one channel for a notification. This table is the service's internal outbox:
/// delivery workers claim due rows, hand them to the channel and record the outcome.
/// </summary>
public sealed class Delivery
{
    public long Id { get; init; }
    public required string EventId { get; init; }
    public required string RecipientId { get; init; }
    public required string Channel { get; init; }

    public DeliveryStatus Status { get; set; }

    /// <summary>Number of times the row was claimed for sending.</summary>
    public int Attempts { get; set; }

    /// <summary>
    /// When the row needs attention next: retry time (Pending), ack deadline (AwaitingAck),
    /// lease expiry (Processing) or the time it was parked (Parked).
    /// </summary>
    public DateTimeOffset NextAttemptAt { get; set; }

    public string? LastError { get; set; }
    public DateTimeOffset? DeliveredAt { get; set; }
    public DateTimeOffset CreatedAt { get; init; }

    public Notification? Notification { get; init; }
}
