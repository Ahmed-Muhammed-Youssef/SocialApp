namespace Notifications.API.Entities;

/// <summary>A notification sent by a producer. Stored once and shared by all its recipients.</summary>
public sealed class Notification
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>The producer that sent this. Together with <see cref="EventId"/> it identifies the event.</summary>
    public required string Source { get; init; }

    /// <summary>The producer's idempotency key; unique per <see cref="Source"/>, so sending the same event twice stores it once.</summary>
    public required string EventId { get; init; }

    /// <summary>The kind of notification (e.g. <c>direct-message</c>), passed to clients.</summary>
    public required string Type { get; init; }

    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>Optional JSON passed to clients unchanged.</summary>
    public string? Data { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public List<UserNotification> Recipients { get; init; } = [];
}
