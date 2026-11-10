namespace Notifications.API.Delivery;

/// <summary>Which delivery attempt a channel is making, alongside the <see cref="DeliveryMessage"/>.</summary>
public sealed record DeliveryContext
{
    /// <summary>
    /// The <see cref="NotificationDelivery"/> row's id. Stable across retries, so channels can pass it to
    /// providers as an idempotency key and a retried send is not delivered twice.
    /// </summary>
    public required Guid DeliveryId { get; init; }

    /// <summary>The attempt being made, starting at 1 (the row's <see cref="NotificationDelivery.Attempts"/> + 1).</summary>
    public required int Attempt { get; init; }
}
