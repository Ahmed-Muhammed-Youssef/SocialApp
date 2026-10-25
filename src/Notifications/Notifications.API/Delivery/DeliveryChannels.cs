namespace Notifications.API.Delivery;

/// <summary>Channel names as stored in <see cref="NotificationDelivery.Channel"/>; they must stay stable once rows exist.</summary>
public static class DeliveryChannels
{
    public const string SignalR = "signalr";
}
