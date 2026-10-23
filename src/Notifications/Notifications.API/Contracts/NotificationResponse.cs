namespace Notifications.API.Contracts;

/// <summary>A single notification as returned by <c>GET /notifications</c>.</summary>
public sealed record NotificationResponse
{
    public required Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Title { get; init; }
    public string? Body { get; init; }
    public string? Data { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>Null when the notification has not been read yet.</summary>
    public DateTimeOffset? ReadAt { get; init; }
}
