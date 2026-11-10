namespace Notifications.API.Delivery;

/// <summary>The outcome of one delivery attempt; the worker turns it into a row update.</summary>
public abstract record DeliveryResult
{
    private DeliveryResult() { }   // only the cases below can derive

    /// <summary>The channel accepted the notification.</summary>
    public sealed record Delivered : DeliveryResult;

    /// <summary>A transient failure (timeout, 5xx, rate limit); try again later, after RetryAfter if given.</summary>
    public sealed record RetryLater(string Error, TimeSpan? RetryAfter = null) : DeliveryResult;

    /// <summary>A permanent failure (invalid address, no device token); retrying cannot succeed.</summary>
    public sealed record GiveUp(string Error) : DeliveryResult;
}
