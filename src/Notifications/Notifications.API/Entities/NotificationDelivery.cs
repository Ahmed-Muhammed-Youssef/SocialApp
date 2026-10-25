namespace Notifications.API.Entities;

/// <summary>
/// The delivery of one <see cref="Entities.Notification"/> through one channel (the outbox row).
/// Written in the same transaction as the notification, so a stored notification always has its deliveries queued.
/// </summary>
public sealed class NotificationDelivery
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public Guid NotificationId { get; init; }

    /// <summary>The channel that delivers it (e.g. <c>signalr</c>). Unique per notification.</summary>
    public required string Channel { get; init; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;

    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the delivery reached a final status; null while it is <see cref="DeliveryStatus.Pending"/>.</summary>
    public DateTimeOffset? ProcessedAt { get; set; }

    /// <summary>How many times delivery has been tried and failed.</summary>
    public int Attempts { get; set; }

    /// <summary>The earliest time the delivery may be tried (again).</summary>
    public DateTimeOffset NextAttemptAt { get; set; }

    /// <summary>The error from the most recent failed attempt.</summary>
    public string? LastError { get; set; }

    /// <summary>After this time the delivery is no longer worth sending; null means it never expires.</summary>
    public DateTimeOffset? ExpiresAt { get; init; }

    public Notification? Notification { get; init; }
}
