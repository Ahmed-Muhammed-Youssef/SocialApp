using System.Net;
using System.Net.Http.Json;
using Notifications.API.Contracts;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Endpoints;

/// <summary>
/// Unit/endpoint tests for <c>GET /notifications</c> — includes authorization, pagination,
/// newest-first ordering, read-state mapping, and recipient isolation.
/// </summary>
[Collection(nameof(NotificationsTestCollection))]
public sealed class ListNotificationsEndpointTests(NotificationsWebAppFactory factory) : NotificationsTestFixture(factory)
{
    // ──────────────────────────────────────────────────────────────
    //  Authorization
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task List_ShouldReturn401_WhenUnauthenticated()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.GetAsync(
            Routes.List, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ──────────────────────────────────────────────────────────────
    //  List Operations
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task List_ShouldReturnEmptyPage_WhenUserHasNoNotifications()
    {
        // Arrange — unique user that has never received a notification
        string userId = $"empty-{Guid.NewGuid():N}";
        HttpClient client = CreateAuthenticatedClient(userId);

        // Act
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(page);
        Assert.Empty(page.Items);
        Assert.Equal(0, page.TotalCount);
    }

    [Fact]
    public async Task List_ShouldReturnOnlyMyNotifications()
    {
        // Arrange — push one notification to "me" and one to "other"
        string myId = $"me-{Guid.NewGuid():N}";
        string otherId = $"other-{Guid.NewGuid():N}";

        await PushNotificationAsync("for-me", myId);
        await PushNotificationAsync("for-other", otherId);

        // Act
        HttpClient client = CreateAuthenticatedClient(myId);
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(page);
        Assert.All(page.Items, n => Assert.Equal("for-me", n.Title));
    }

    [Fact]
    public async Task List_ShouldReturnNewestFirst()
    {
        // Arrange
        string userId = $"order-{Guid.NewGuid():N}";

        await PushNotificationAsync("First", userId);
        await Task.Delay(50, TestContext.Current.CancellationToken);
        await PushNotificationAsync("Second", userId);
        await Task.Delay(50, TestContext.Current.CancellationToken);
        await PushNotificationAsync("Third", userId);

        // Act
        HttpClient client = CreateAuthenticatedClient(userId);
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(page);
        Assert.True(page.Items.Count >= 3);

        int indexThird = page.Items.FindIndex(n => n.Title == "Third");
        int indexSecond = page.Items.FindIndex(n => n.Title == "Second");
        int indexFirst = page.Items.FindIndex(n => n.Title == "First");

        Assert.True(indexThird < indexSecond);
        Assert.True(indexSecond < indexFirst);
    }

    [Fact]
    public async Task List_ShouldSupportPagination()
    {
        // Arrange — push 5 notifications, request page size 2
        string userId = $"paged-{Guid.NewGuid():N}";
        for (int i = 0; i < 5; i++)
        {
            await PushNotificationAsync($"Paged-{i}", userId);
        }

        HttpClient client = CreateAuthenticatedClient(userId);

        // Act — page 1
        var page1 = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            $"{Routes.List}?page=1&pageSize=2", TestContext.Current.CancellationToken);

        // Act — page 2
        var page2 = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            $"{Routes.List}?page=2&pageSize=2", TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(page1);
        Assert.NotNull(page2);

        Assert.Equal(2, page1.Items.Count);
        Assert.True(page1.HasNextPage);

        Assert.Equal(2, page2.Items.Count);

        Assert.Equal(5, page1.TotalCount);
        Assert.Equal(5, page2.TotalCount);

        // No overlapping items between pages
        var ids1 = page1.Items.Select(n => n.Id).ToHashSet();
        var ids2 = page2.Items.Select(n => n.Id).ToHashSet();
        Assert.Empty(ids1.Intersect(ids2));
    }

    [Fact]
    public async Task List_ShouldIncludeReadState()
    {
        // Arrange — push, then mark one as read
        string userId = $"readstate-{Guid.NewGuid():N}";
        await PushNotificationAsync("unread-notif", userId);
        await PushNotificationAsync("read-notif", userId);

        HttpClient client = CreateAuthenticatedClient(userId);

        // Find the "read-notif" and mark it read
        var initial = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);
        Assert.NotNull(initial);
        NotificationResponse toRead = initial.Items.First(n => n.Title == "read-notif");
        await client.PatchAsync(Routes.MarkRead(toRead.Id), null, TestContext.Current.CancellationToken);

        // Act — list again
        var page = await client.GetFromJsonAsync<PagedResponse<NotificationResponse>>(
            Routes.List, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(page);
        NotificationResponse readNotif = page.Items.First(n => n.Title == "read-notif");
        NotificationResponse unreadNotif = page.Items.First(n => n.Title == "unread-notif");

        Assert.NotNull(readNotif.ReadAt);
        Assert.Null(unreadNotif.ReadAt);
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
