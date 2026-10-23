using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notifications.API.Hubs;
using Notifications.API.Contracts;
using Notifications.API.Entities;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Features;

/// <summary>
/// Tests for <c>POST /internal/notifications</c> — the producer-facing endpoint that persists a
/// notification and fans it out to the given recipient list.
/// </summary>
[Collection(nameof(NotificationsTestCollection))]
public sealed class PushTests(NotificationsWebAppFactory factory) : NotificationsTestFixture(factory)
{
    // ──────────────────────────────────────────────────────────────
    //  Happy path
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task Push_ShouldReturn201_WhenPayloadIsValid()
    {
        // Arrange
        HttpClient client = CreateClient();
        var request = MakeRequest();

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldFanOut_ToEveryRecipient()
    {
        // Arrange
        string userId1 = $"fan-user-{Guid.NewGuid():N}";
        string userId2 = $"fan-user-{Guid.NewGuid():N}";

        HttpClient pushClient = CreateClient();
        var request = MakeRequest(recipientIds: [userId1, userId2]);

        // Act — push one notification targeting two users
        HttpResponseMessage pushResponse = await pushClient.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);
        pushResponse.EnsureSuccessStatusCode();

        // Assert — each user should see the notification in their list
        List<UserNotification> stored1 = await GetStoredNotificationsAsync(userId1);
        List<UserNotification> stored2 = await GetStoredNotificationsAsync(userId2);

        Assert.Contains(stored1, un => un.Notification!.Title == request.Title);
        Assert.Contains(stored2, un => un.Notification!.Title == request.Title);
    }

    [Fact]
    public async Task Push_ShouldStillReturn201AndPersist_WhenTheLivePushFails()
    {
        // Arrange
        string userId = $"hubdown-user-{Guid.NewGuid():N}";
        using WebApplicationFactory<Program> hubDownFactory = Factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IHubContext<NotificationsHub, INotificationsClient>>();
                services.AddSingleton<IHubContext<NotificationsHub, INotificationsClient>>(new ThrowingHubContext());
            }));

        HttpClient producer = hubDownFactory.CreateClient();
        var request = MakeRequest(recipientIds: [userId]);

        // Act
        HttpResponseMessage response = await producer.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        List<UserNotification> stored = await GetStoredNotificationsAsync(userId);

        Assert.Single(stored, un => un.Notification!.Title == request.Title);
    }

    // ──────────────────────────────────────────────────────────────
    //  Idempotency
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task Push_ShouldBeIdempotent_WhenSameEventIdSentTwice()
    {
        // Arrange
        string userId = $"idem-user-{Guid.NewGuid():N}";
        HttpClient pushClient = CreateClient();
        var request = MakeRequest(recipientIds: [userId]);

        // Act — send the exact same event twice
        HttpResponseMessage first = await pushClient.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);
        HttpResponseMessage second = await pushClient.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        first.EnsureSuccessStatusCode();
        second.EnsureSuccessStatusCode(); // should succeed (not 500/409)

        // Assert — the user should see exactly ONE notification, not two
        List<UserNotification> stored = await GetStoredNotificationsAsync(userId);

        Assert.Equal(1, stored.Count(un => un.Notification!.Title == request.Title));
    }

    [Fact]
    public async Task Push_ShouldStoreBoth_WhenDifferentSourcesReuseTheSameEventId()
    {
        // Arrange
        string userId = $"src-user-{Guid.NewGuid():N}";
        string eventId = $"evt-{Guid.NewGuid():N}";
        HttpClient client = CreateClient();

        // Act
        HttpResponseMessage first = await client.PostAsJsonAsync(
            Routes.Push,
            MakeRequest(eventId: eventId, title: "from A", recipientIds: [userId], source: "producer-a"),
            TestContext.Current.CancellationToken);
        HttpResponseMessage second = await client.PostAsJsonAsync(
            Routes.Push,
            MakeRequest(eventId: eventId, title: "from B", recipientIds: [userId], source: "producer-b"),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Created, second.StatusCode);

        List<UserNotification> stored = await GetStoredNotificationsAsync(userId);

        Assert.Equal(2, stored.Count);
    }

    [Fact]
    public async Task Push_ShouldStoreType()
    {
        // Arrange
        string userId = $"type-user-{Guid.NewGuid():N}";
        HttpClient client = CreateClient();

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push,
            MakeRequest(recipientIds: [userId], type: "direct-message"),
            TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        // Assert
        List<UserNotification> stored = await GetStoredNotificationsAsync(userId);

        Assert.Equal("direct-message", Assert.Single(stored).Notification!.Type);
    }

    // ──────────────────────────────────────────────────────────────
    //  Validation
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task Push_ShouldReturn400_WhenSourceIsMissing()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, MakeRequest(source: ""), TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenTypeIsMissing()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, MakeRequest(type: ""), TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenSourceOrTypeIsTooLong()
    {
        HttpClient client = CreateClient();

        HttpResponseMessage tooLongSource = await client.PostAsJsonAsync(
            Routes.Push,
            MakeRequest(source: new string('s', NotificationLimits.SourceMaxLength + 1)),
            TestContext.Current.CancellationToken);
        HttpResponseMessage tooLongType = await client.PostAsJsonAsync(
            Routes.Push,
            MakeRequest(type: new string('t', NotificationLimits.TypeMaxLength + 1)),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, tooLongSource.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, tooLongType.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenTitleIsMissing()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(title: "");

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenRecipientsIsEmpty()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(recipientIds: []);

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenEventIdIsMissing()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(eventId: "");

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenDataIsNotValidJson()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(data: "{not json");

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenTitleIsTooLong()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(title: new string('t', NotificationLimits.TitleMaxLength + 1));

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenBodyIsTooLong()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(body: new string('b', NotificationLimits.BodyMaxLength + 1));

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldReturn400_WhenARecipientIdIsTooLong()
    {
        HttpClient client = CreateClient();
        var request = MakeRequest(recipientIds: [new string('u', NotificationLimits.RecipientIdMaxLength + 1)]);

        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Push_ShouldStoreOnce_WhenARecipientIsListedTwice()
    {
        // Arrange
        string userId = $"dup-user-{Guid.NewGuid():N}";
        HttpClient client = CreateClient();
        var request = MakeRequest(recipientIds: [userId, userId]);

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            Routes.Push, request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        List<UserNotification> stored = await GetStoredNotificationsAsync(userId);

        Assert.Single(stored, un => un.Notification!.Title == request.Title);
    }

    // ──────────────────────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────────────────────

    private static PushNotificationRequest MakeRequest(
        string? eventId = null,
        string title = "New friend request",
        string? body = "Alice wants to be your friend.",
        string? data = null,
        List<string>? recipientIds = null,
        string source = "test-producer",
        string type = "friend-request")
    {
        return new PushNotificationRequest
        {
            Source = source,
            EventId = eventId ?? $"evt-{Guid.NewGuid():N}",
            Type = type,
            Title = title,
            Body = body,
            Data = data,
            RecipientIds = recipientIds ?? [$"user-{Guid.NewGuid():N}"],
        };
    }
}
