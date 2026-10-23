namespace Notifications.API.Hubs;

public interface INotificationsClient
{
    Task ReceiveNotification(NotificationResponse notification);
}
