namespace Notifications.API.Delivery;

/// <summary>
/// A way of getting a stored notification to its recipients (live socket, email, mobile push...).
/// Delivery is at-least-once: a crash between sending and recording the result sends it again, so a channel
/// must tolerate duplicates (e.g. pass <see cref="DeliveryContext.DeliveryId"/> to the provider as an idempotency key).
/// </summary>
public interface IDeliveryChannel
{
    /// <summary>Stored in <see cref="NotificationDelivery.Channel"/>, so it must never change once rows exist.</summary>
    string Name { get; }

    /// <summary>The channel's defaults; configuration may override them.</summary>
    DeliveryPolicy Policy { get; }

    /// <summary>
    /// Makes one attempt and says how it went; the worker handles retries, so a channel never loops or waits.
    /// An unexpected exception is treated as <see cref="DeliveryResult.RetryLater"/>; cancellation is left to propagate.
    /// </summary>
    Task<DeliveryResult> DeliverAsync(DeliveryMessage message, DeliveryContext context, CancellationToken ct);
}
