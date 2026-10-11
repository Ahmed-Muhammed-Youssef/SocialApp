namespace Notifications.API.Entities;

/// <summary>
/// Stored as text. The partial indexes in <c>DeliveryConfigurations</c> reference these names, so renaming one needs a migration.
/// </summary>
public enum DeliveryStatus
{
    /// <summary>Waiting to be claimed once <c>NextAttemptAt</c> is reached.</summary>
    Pending,

    /// <summary>Claimed by a worker; reclaimable when the lease (<c>NextAttemptAt</c>) expires.</summary>
    Processing,

    /// <summary>Sent; waiting for the client to acknowledge it before the deadline (<c>NextAttemptAt</c>).</summary>
    AwaitingAck,

    /// <summary>Recipient unreachable on this channel; resumed when they become available again.</summary>
    Parked,

    Delivered,
    Failed,
    Expired,
}
