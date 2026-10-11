namespace Notifications.API.Common;

/// <summary>Source-generated log messages (no boxing or formatting when the level is disabled).</summary>
internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Delivery worker for channel {Channel} started")]
    public static partial void WorkerStarted(this ILogger logger, string channel);

    [LoggerMessage(Level = LogLevel.Information, Message = "Delivery worker for channel {Channel} is disabled on this host")]
    public static partial void WorkerDisabled(this ILogger logger, string channel);

    [LoggerMessage(Level = LogLevel.Error, Message = "Delivery batch for channel {Channel} failed")]
    public static partial void BatchFailed(this ILogger logger, Exception exception, string channel);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to process delivery {DeliveryId} on channel {Channel}")]
    public static partial void DeliveryProcessingFailed(this ILogger logger, Exception exception, long deliveryId, string channel);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Delivery {DeliveryId} of {EventId} to {RecipientId} on {Channel} failed: {Error}")]
    public static partial void DeliveryFailed(this ILogger logger, long deliveryId, string eventId, string recipientId, string channel, string? error);

    [LoggerMessage(Level = LogLevel.Information, Message = "Notification {EventId} was already accepted; ignoring duplicate push")]
    public static partial void DuplicatePush(this ILogger logger, string eventId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Delivery cleanup failed")]
    public static partial void CleanupFailed(this ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "Delivery cleanup expired {Overdue} overdue and {StaleParked} stale parked deliveries, deleted {Deleted} finished notifications")]
    public static partial void CleanupCompleted(this ILogger logger, int overdue, int staleParked, int deleted);
}
