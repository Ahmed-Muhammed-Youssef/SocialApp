namespace Notifications.API.Entities;

/// <summary>One recipient's copy of a <see cref="Entities.Notification"/>, with their read state.</summary>
public sealed class UserNotification
{
    public Guid NotificationId { get; init; }

    /// <summary>The user id as it appears in the JWT's <c>nameid</c> claim.</summary>
    public required string RecipientId { get; init; }

    /// <summary>Copied from the notification so a user's list can be sorted without a join.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? ReadAt { get; set; }

    public Notification? Notification { get; init; }
}
