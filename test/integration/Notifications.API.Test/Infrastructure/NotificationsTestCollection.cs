namespace Notifications.API.Test.Infrastructure;

[CollectionDefinition(nameof(NotificationsTestCollection))]
public sealed class NotificationsTestCollection : ICollectionFixture<NotificationsWebAppFactory>;
