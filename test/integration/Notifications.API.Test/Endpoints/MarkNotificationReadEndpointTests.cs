using System.Net;
using System.Net.Http.Json;
using Notifications.API.Contracts;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Endpoints;

/// <summary>
/// Unit/endpoint tests for <c>PATCH /notifications/{id}/read</c> — includes authorization,
/// marking read, idempotency, and 404 security checks for non-existent / other-user notifications.
/// </summary>
[Collection(nameof(NotificationsTestCollection))]
public sealed class MarkNotificationReadEndpointTests(NotificationsWebAppFactory factory) : NotificationsTestFixture(factory)
{
    // ──────────────────────────────────────────────────────────────
    //  Authorization
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task MarkRead_ShouldReturn401_WhenUnauthenticated()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.PatchAsync(
            Routes.MarkRead(Guid.NewGuid()), null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ──────────────────────────────────────────────────────────────
    //  Mark Read Operations
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task MarkRead_ShouldReturn204_AndSetReadAt()
    {
        // Arrange
        string userId = $"read-{Guid.NewGuid():N}";
        await PushNotificationAsync("to-read", userId);

        HttpClient client = CreateAuthenticatedClient(userId);
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(page);
        Guid notifId = page.Items.First(n => n.Title == "to-read").Id;

        // Act
        HttpResponseMessage response = await client.PatchAsync(
            Routes.MarkRead(notifId), null, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify ReadAt is now set
        var refreshed = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(refreshed);
        Assert.NotNull(refreshed.Items.First(n => n.Id == notifId).ReadAt);
    }

    [Fact]
    public async Task MarkRead_ShouldBeIdempotent_WhenCalledTwice()
    {
        // Arrange
        string userId = $"idem-read-{Guid.NewGuid():N}";
        await PushNotificationAsync("idem-read-test", userId);

        HttpClient client = CreateAuthenticatedClient(userId);
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(page);
        Guid notifId = page.Items.First().Id;

        // Act
        HttpResponseMessage first = await client.PatchAsync(
            Routes.MarkRead(notifId), null, TestContext.Current.CancellationToken);
        HttpResponseMessage second = await client.PatchAsync(
            Routes.MarkRead(notifId), null, TestContext.Current.CancellationToken);

        // Assert — both succeed
        Assert.Equal(HttpStatusCode.NoContent, first.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, second.StatusCode);
    }

    [Fact]
    public async Task MarkRead_ShouldReturn404_WhenNotificationDoesNotExist()
    {
        HttpClient client = CreateAuthenticatedClient($"user-{Guid.NewGuid():N}");

        HttpResponseMessage response = await client.PatchAsync(
            Routes.MarkRead(Guid.NewGuid()), null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task MarkRead_ShouldReturn404_WhenNotificationBelongsToAnotherUser()
    {
        // Arrange — push to user-A, try to read as user-B
        string userA = $"owner-{Guid.NewGuid():N}";
        string userB = $"intruder-{Guid.NewGuid():N}";

        await PushNotificationAsync("private-notif", userA);

        HttpClient ownerClient = CreateAuthenticatedClient(userA);
        var page = await ownerClient.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(page);
        Guid notifId = page.Items.First().Id;

        // Act — user-B tries to mark user-A's notification
        HttpClient intruderClient = CreateAuthenticatedClient(userB);
        HttpResponseMessage response = await intruderClient.PatchAsync(
            Routes.MarkRead(notifId), null, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
