using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Notifications.API.Contracts;
using Notifications.API.Data;
using Notifications.API.Endpoints;
using Notifications.API.Hubs;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Notifications.API.Test.Endpoints;

/// <summary>
/// Fast unit tests for <see cref="PushNotificationEndpoint.HandleAsync"/>.
/// Tests SignalR broadcasting and DB persistence directly with NO Docker and NO WebSockets needed.
/// </summary>
public sealed class PushNotificationHandlerUnitTests : IAsyncDisposable
{
    private readonly NotificationsDbContext _db;
    private readonly IHubContext<NotificationsHub, INotificationsClient> _hubContext;
    private readonly IHubClients<INotificationsClient> _hubClients;
    private readonly INotificationsClient _mockClient;

    public PushNotificationHandlerUnitTests()
    {
        // 1. In-memory DB for instant, zero-dependency unit tests
        var options = new DbContextOptionsBuilder<NotificationsDbContext>()
            .UseInMemoryDatabase(databaseName: $"UnitTestsDb_{Guid.NewGuid():N}")
            .Options;

        _db = new NotificationsDbContext(options);

        // 2. Mock SignalR HubContext using NSubstitute
        _hubContext = Substitute.For<IHubContext<NotificationsHub, INotificationsClient>>();
        _hubClients = Substitute.For<IHubClients<INotificationsClient>>();
        _mockClient = Substitute.For<INotificationsClient>();

        _hubContext.Clients.Returns(_hubClients);
        _hubClients.Users(Arg.Any<IReadOnlyList<string>>()).Returns(_mockClient);
    }

    [Fact]
    public async Task HandleAsync_ShouldBroadcastToSignalR_AndPersistNotification()
    {
        // Arrange
        var request = new PushNotificationRequest
        {
            Source = "social-api",
            EventId = $"evt-{Guid.NewGuid():N}",
            Type = "direct-message",
            Title = "John Doe sent a message",
            Body = "Hey there!",
            Data = "{\"messageId\": 42}",
            RecipientIds = ["user-1", "user-2"],
        };

        // Act
        Results<Created, Ok> result = await PushNotificationEndpoint.HandleAsync(
            request, _db, _hubContext, NullLoggerFactory.Instance, TestContext.Current.CancellationToken);

        // Assert 1: Result is 201 Created
        Assert.IsType<Created>(result.Result);

        // Assert 2: Database contains the Notification and UserNotification rows
        var saved = await _db.Notifications
            .Include(n => n.Recipients)
            .FirstOrDefaultAsync(n => n.EventId == request.EventId, TestContext.Current.CancellationToken);

        Assert.NotNull(saved);
        Assert.Equal(2, saved.Recipients.Count);

        // Assert 3: SignalR broadcast was sent to the exact recipients with the notification DTO
        await _mockClient.Received(1).ReceiveNotification(
            Arg.Is<NotificationResponse>(n =>
                n.Title == request.Title &&
                n.Type == request.Type &&
                n.Body == request.Body &&
                n.Data == request.Data));
    }

    [Fact]
    public async Task HandleAsync_ShouldBeIdempotent_AndNotBroadcastAgain_WhenDuplicateEventReceived()
    {
        // Arrange
        var request = new PushNotificationRequest
        {
            Source = "social-api",
            EventId = "duplicate-event-id",
            Type = "friend-request",
            Title = "Friend Request",
            RecipientIds = ["user-1"],
        };

        // First call
        await PushNotificationEndpoint.HandleAsync(
            request, _db, _hubContext, NullLoggerFactory.Instance, TestContext.Current.CancellationToken);

        _mockClient.ClearReceivedCalls();

        // Act: Second call with exact same (Source, EventId)
        Results<Created, Ok> secondResult = await PushNotificationEndpoint.HandleAsync(
            request, _db, _hubContext, NullLoggerFactory.Instance, TestContext.Current.CancellationToken);

        // Assert: Returns 200 Ok and did NOT broadcast over SignalR a second time
        Assert.IsType<Ok>(secondResult.Result);
        await _mockClient.DidNotReceive().ReceiveNotification(Arg.Any<NotificationResponse>());
    }

    [Fact]
    public async Task HandleAsync_ShouldStillReturnCreated_WhenSignalRBroadcastFails()
    {
        // Arrange: Simulate SignalR failure
        _mockClient.ReceiveNotification(Arg.Any<NotificationResponse>())
            .ThrowsAsync(new InvalidOperationException("SignalR connection reset"));

        var request = new PushNotificationRequest
        {
            Source = "social-api",
            EventId = $"evt-{Guid.NewGuid():N}",
            Type = "test-type",
            Title = "Resilience test",
            RecipientIds = ["user-1"],
        };

        // Act: Should not throw
        Results<Created, Ok> result = await PushNotificationEndpoint.HandleAsync(
            request, _db, _hubContext, NullLoggerFactory.Instance, TestContext.Current.CancellationToken);

        // Assert: Notification is still saved and 201 is returned
        Assert.IsType<Created>(result.Result);
        var stored = await _db.Notifications.AnyAsync(n => n.EventId == request.EventId, TestContext.Current.CancellationToken);
        Assert.True(stored);
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
    }
}
