using System.Net;
using System.Net.Http.Json;
using Notifications.API.Contracts;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Endpoints;

/// <summary>
/// Unit/endpoint tests for <c>GET /notifications/unread-count</c> — includes authorization,
/// empty-state counting, count increments on push, and decrements on read / mark-all-read.
/// </summary>
[Collection(nameof(NotificationsTestCollection))]
public sealed class GetUnreadCountEndpointTests(NotificationsWebAppFactory factory) : NotificationsTestFixture(factory)
{
    // ──────────────────────────────────────────────────────────────
    //  Authorization
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task UnreadCount_ShouldReturn401_WhenUnauthenticated()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.GetAsync(
            Routes.UnreadCount, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ──────────────────────────────────────────────────────────────
    //  Count Operations
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task UnreadCount_ShouldReturn0_WhenUserHasNoNotifications()
    {
        // Arrange
        string userId = $"zero-{Guid.NewGuid():N}";
        HttpClient client = CreateAuthenticatedClient(userId);

        // Act
        var result = await client.GetFromJsonAsync<UnreadCountResponse>(
            Routes.UnreadCount, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public async Task UnreadCount_ShouldReturnCorrectCount_AfterPush()
    {
        // Arrange
        string userId = $"count-{Guid.NewGuid():N}";
        await PushNotificationAsync("n1", userId);
        await PushNotificationAsync("n2", userId);
        await PushNotificationAsync("n3", userId);

        HttpClient client = CreateAuthenticatedClient(userId);

        // Act
        var result = await client.GetFromJsonAsync<UnreadCountResponse>(
            Routes.UnreadCount, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task UnreadCount_ShouldDecrease_AfterMarkingOneAsRead()
    {
        // Arrange — push 3, mark 1 as read
        string userId = $"dec-{Guid.NewGuid():N}";
        await PushNotificationAsync("d1", userId);
        await PushNotificationAsync("d2", userId);
        await PushNotificationAsync("d3", userId);

        HttpClient client = CreateAuthenticatedClient(userId);

        // Read one
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(page);
        Guid firstId = page.Items.First().Id;
        await client.PatchAsync(Routes.MarkRead(firstId), null, TestContext.Current.CancellationToken);

        // Act
        var result = await client.GetFromJsonAsync<UnreadCountResponse>(
            Routes.UnreadCount, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UnreadCount_ShouldReturn0_AfterMarkAllRead()
    {
        // Arrange
        string userId = $"allzero-{Guid.NewGuid():N}";
        await PushNotificationAsync("a1", userId);
        await PushNotificationAsync("a2", userId);

        HttpClient client = CreateAuthenticatedClient(userId);
        await client.PostAsync(Routes.MarkAllRead, null, TestContext.Current.CancellationToken);

        // Act
        var result = await client.GetFromJsonAsync<UnreadCountResponse>(
            Routes.UnreadCount, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Count);
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
