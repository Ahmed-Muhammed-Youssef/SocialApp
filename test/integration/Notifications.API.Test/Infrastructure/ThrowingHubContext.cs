using Microsoft.AspNetCore.SignalR;
using Notifications.API.Contracts;
using Notifications.API.Hubs;

namespace Notifications.API.Test.Infrastructure;

/// <summary>A hub context whose every live push fails — simulates SignalR being unavailable after a notification is saved.</summary>
internal sealed class ThrowingHubContext : IHubContext<NotificationsHub, INotificationsClient>
{
    public IHubClients<INotificationsClient> Clients { get; } = new ThrowingHubClients();

    public IGroupManager Groups => throw new NotSupportedException();

    private sealed class ThrowingHubClients : IHubClients<INotificationsClient>
    {
        private static readonly INotificationsClient Client = new ThrowingClient();

        public INotificationsClient All => Client;
        public INotificationsClient AllExcept(IReadOnlyList<string> excludedConnectionIds) => Client;
        INotificationsClient IHubClients<INotificationsClient>.Client(string connectionId) => Client;
        public INotificationsClient Clients(IReadOnlyList<string> connectionIds) => Client;
        public INotificationsClient Group(string groupName) => Client;
        public INotificationsClient GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => Client;
        public INotificationsClient Groups(IReadOnlyList<string> groupNames) => Client;
        public INotificationsClient User(string userId) => Client;
        public INotificationsClient Users(IReadOnlyList<string> userIds) => Client;
    }

    private sealed class ThrowingClient : INotificationsClient
    {
        public Task ReceiveNotification(NotificationResponse notification) =>
            throw new InvalidOperationException("SignalR is down.");
    }
}
