namespace Notifications.API.Delivery;

/// <summary>How the worker treats one channel's deliveries. Each channel declares its defaults in code.</summary>
public sealed record DeliveryPolicy
{
    /// <summary>How long after the notification is stored it is still worth delivering; null means it never expires.</summary>
    public TimeSpan? TimeToLive { get; init; }

    /// <summary>Attempts (the first included) after which a delivery that keeps failing is marked Failed.</summary>
    public required int MaxAttempts { get; init; }
}
