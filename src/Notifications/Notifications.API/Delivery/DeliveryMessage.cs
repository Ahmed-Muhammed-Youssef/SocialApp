namespace Notifications.API.Delivery;

/// <summary>What a channel delivers: a read-only copy of a stored notification and who it is for.</summary>
public sealed record DeliveryMessage
{
    public required Guid NotificationId { get; init; }
    public required string Type { get; init; }
    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>Optional JSON, passed to clients unchanged.</summary>
    public string? Data { get; init; }

    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>User ids as they appear in the JWT's <c>nameid</c> claim.</summary>
    public required IReadOnlyList<string> RecipientIds { get; init; }
}
