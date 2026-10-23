using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notifications.API.Hubs;

[Authorize]
public sealed class NotificationsHub : Hub<INotificationsClient>
{
}
