namespace Notifications.API.Entities;

/// <summary>
/// A notification accepted by the push endpoint. <see cref="EventId"/> is the producer's idempotency key (the inbox key).
/// </summary>
public sealed class Notification
{
    public required string EventId { get; init; }
    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>Raw JSON payload stored as <c>jsonb</c>.</summary>
    public string? Data { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
    public DateTimeOffset ReceivedAt { get; init; }

    public List<Delivery> Deliveries { get; init; } = [];
}
