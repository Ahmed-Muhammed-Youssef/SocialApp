using System.Net;
using System.Net.Http.Json;
using Notifications.API.Contracts;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Endpoints;

/// <summary>
/// Unit/endpoint tests for <c>POST /notifications/read-all</c> — includes authorization,
/// batch marking read, user isolation, and no-op handling when no unread notifications exist.
/// </summary>
[Collection(nameof(NotificationsTestCollection))]
public sealed class MarkAllNotificationsReadEndpointTests(NotificationsWebAppFactory factory) : NotificationsTestFixture(factory)
{
    // ──────────────────────────────────────────────────────────────
    //  Authorization
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task MarkAllRead_ShouldReturn401_WhenUnauthenticated()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.PostAsync(
            Routes.MarkAllRead, null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ──────────────────────────────────────────────────────────────
    //  Mark All Read Operations
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task MarkAllRead_ShouldReturn204_AndSetReadAtOnEveryNotification()
    {
        // Arrange — push 3 unread notifications
        string userId = $"all-read-{Guid.NewGuid():N}";
        await PushNotificationAsync("allread-1", userId);
        await PushNotificationAsync("allread-2", userId);
        await PushNotificationAsync("allread-3", userId);

        HttpClient client = CreateAuthenticatedClient(userId);

        // Act
        HttpResponseMessage response = await client.PostAsync(
            Routes.MarkAllRead, null, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(page);
        Assert.All(page.Items, n => Assert.NotNull(n.ReadAt));
    }

    [Fact]
    public async Task MarkAllRead_ShouldNotAffectOtherUsers()
    {
        // Arrange
        string meId = $"me-allread-{Guid.NewGuid():N}";
        string otherId = $"other-allread-{Guid.NewGuid():N}";

        await PushNotificationAsync("mine", meId);
        await PushNotificationAsync("theirs", otherId);

        HttpClient myClient = CreateAuthenticatedClient(meId);
        HttpClient otherClient = CreateAuthenticatedClient(otherId);

        // Act — I mark all read
        await myClient.PostAsync(Routes.MarkAllRead, null, TestContext.Current.CancellationToken);

        // Assert — other user's notifications remain unread
        var otherPage = await otherClient.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(otherPage);
        Assert.All(otherPage.Items, n => Assert.Null(n.ReadAt));
    }

    [Fact]
    public async Task MarkAllRead_ShouldSucceed_WhenUserHasNoUnreadNotifications()
    {
        // Arrange — user with no notifications at all
        string userId = $"no-notif-{Guid.NewGuid():N}";
        HttpClient client = CreateAuthenticatedClient(userId);

        // Act — should be a no-op, not an error
        HttpResponseMessage response = await client.PostAsync(
            Routes.MarkAllRead, null, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    // ──────────────────────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────────────────────

    private async Task PushNotificationAsync(string title, string recipientId)
    {
        HttpClient client = CreateClient();
        var request = new PushNotificationRequest
        {
            Source = "test-producer",
            EventId = $"evt-{Guid.NewGuid():N}",
            Type = "test",
            Title = title,
            Body = null,
            RecipientIds = [recipientId],
        };
        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
