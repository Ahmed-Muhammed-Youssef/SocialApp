namespace Notifications.API.Features.Push;

public sealed record PushRequest
{
    /// <summary>Producer-generated idempotency key: re-posting the same EventId is accepted but ignored.</summary>
    public required string EventId { get; init; }
    public required string Title { get; init; }
    public string? Body { get; init; }

    /// <summary>User ids (the JWT <c>nameid</c> claim) to notify. Duplicates are ignored.</summary>
    public required IReadOnlyList<string> Recipients { get; init; }

    /// <summary>Optional JSON payload passed through to clients unchanged.</summary>
    public JsonNode? Data { get; init; }

    /// <summary>When the event happened at the producer; defaults to the time the push is received.</summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>After this time undelivered notifications are dropped instead of sent late.</summary>
    public DateTimeOffset? ExpiresAt { get; init; }
}

public sealed record PushResponse(string EventId, bool Duplicate);
