using Notifications.API.Hubs;

namespace Notifications.API.Endpoints;

/// <summary>
/// Registers all notification endpoints and SignalR hubs with the application.
/// </summary>
public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this WebApplication app)
    {
        app.MapPushNotification();
        app.MapListNotifications();
        app.MapMarkNotificationRead();
        app.MapMarkAllNotificationsRead();
        app.MapGetUnreadCount();

        app.MapHub<NotificationsHub>("/hubs/notifications");
    }
}
