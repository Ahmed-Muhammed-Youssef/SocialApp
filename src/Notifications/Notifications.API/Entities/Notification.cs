namespace Notifications.API.Entities;

/// <summary>A notification sent by a producer. Stored once and shared by all its recipients.</summary>
public sealed class Notification
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>The producer's idempotency key; unique, so sending the same event twice stores it once.</summary>
    public required string EventId { get; init; }

    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>Optional JSON passed to clients unchanged.</summary>
    public string? Data { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public List<UserNotification> Recipients { get; init; } = [];
}
