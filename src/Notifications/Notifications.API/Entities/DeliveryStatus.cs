namespace Notifications.API.Entities;

/// <summary>Where a <see cref="NotificationDelivery"/> is in its lifecycle. Stored as a string.</summary>
public enum DeliveryStatus
{
    /// <summary>
    /// Not delivered yet and still owned by the worker: waiting for its first attempt, or failed with retries left
    /// (then <see cref="NotificationDelivery.Attempts"/> &gt; 0 and <see cref="NotificationDelivery.LastError"/> is set).
    /// The only state the worker picks up.
    /// </summary>
    Pending,

    /// <summary>Final. The channel accepted it (for SignalR: handed to the hub, even if the user had no open connection).</summary>
    Delivered,

    /// <summary>Final. <see cref="NotificationDelivery.ExpiresAt"/> passed before it could be delivered, so it was dropped.</summary>
    Expired,

    /// <summary>Final. Every attempt failed and the retry limit was reached; see <see cref="NotificationDelivery.LastError"/>.</summary>
    Failed,
}
