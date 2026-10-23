namespace Notifications.API.Contracts;

/// <summary>Request sent by a producer (the main API) to fan-out a notification to one or more recipients.</summary>
public sealed record PushNotificationRequest
{
    /// <summary>Who is sending (e.g. <c>social-api</c>). Scopes <see cref="EventId"/>, so two producers can't collide.</summary>
    public required string Source { get; init; }

    /// <summary>Producer-supplied idempotency key, unique per <see cref="Source"/>. Duplicate sends are ignored.</summary>
    public required string EventId { get; init; }

    /// <summary>What kind of notification this is (e.g. <c>direct-message</c>), so clients can group, filter and deep-link.</summary>
    public required string Type { get; init; }

    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>Optional JSON payload passed to clients unchanged (e.g. deep-link data).</summary>
    public string? Data { get; init; }

    /// <summary>User IDs (from the JWT <c>nameid</c> claim) that should receive this notification.</summary>
    public required List<string> RecipientIds { get; init; }
}
