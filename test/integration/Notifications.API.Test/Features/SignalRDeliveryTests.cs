using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.API.Channels.SignalR;
using Notifications.API.Deliveries;
using Notifications.API.Entities;
using Notifications.API.Features.Push;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Features;

public sealed class SignalRDeliveryTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private static readonly TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(15);

    [Fact]
    public async Task Hub_ShouldRejectConnection_WithoutToken()
    {
        // Arrange
        await using HubConnection connection = factory.CreateHubConnection(userId: null);

        // Act
        HttpRequestException exception = await Assert.ThrowsAsync<HttpRequestException>(() => connection.StartAsync(TestContext.Current.CancellationToken));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
    }

    [Fact]
    public async Task Delivery_ShouldPushToConnectedUser_AndMarkDeliveredOnAck()
    {
        // Arrange
        string userId = NewUserId();
        await using ConnectedClient client = await ConnectAsync(userId);
        PushRequest request = WebAppFactory.CreatePushRequest(userId);

        // Act
        await factory.PushAsync(request);
        NotificationMessage message = await client.WaitForMessageAsync(request.EventId);
        await client.Connection.InvokeAsync(nameof(NotificationsHub.Acknowledge), request.EventId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(request.Title, message.Title);
        Assert.Equal(request.Body, message.Body);
        Assert.Equal(42, message.Data?.GetProperty("requesterId").GetInt32());

        Delivery delivery = await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Delivered);
        Assert.Equal(1, delivery.Attempts);
        Assert.NotNull(delivery.DeliveredAt);
    }

    [Fact]
    public async Task Delivery_ShouldResend_WhenAckNotReceivedBeforeTimeout()
    {
        // Arrange
        string userId = NewUserId();
        await using ConnectedClient client = await ConnectAsync(userId);
        PushRequest request = WebAppFactory.CreatePushRequest(userId);

        // Act: don't acknowledge the first send
        await factory.PushAsync(request);
        await client.WaitForMessageAsync(request.EventId, count: 2);
        await client.Connection.InvokeAsync(nameof(NotificationsHub.Acknowledge), request.EventId, TestContext.Current.CancellationToken);

        // Assert
        Delivery delivery = await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Delivered);
        Assert.True(delivery.Attempts >= 2);
    }

    [Fact]
    public async Task Delivery_ShouldPark_WhenRecipientOffline_AndReplayOnConnect()
    {
        // Arrange
        string userId = NewUserId();
        PushRequest request = WebAppFactory.CreatePushRequest(userId);
        await factory.PushAsync(request);

        Delivery parked = await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Parked);
        Assert.Equal(0, parked.Attempts); // being offline doesn't use up attempts

        // Act
        await using ConnectedClient client = await ConnectAsync(userId);
        NotificationMessage message = await client.WaitForMessageAsync(request.EventId);
        await client.Connection.InvokeAsync(nameof(NotificationsHub.Acknowledge), request.EventId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(request.Title, message.Title);
        await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Delivered);
    }

    [Fact]
    public async Task Delivery_ShouldFail_AfterMaxAttemptsWithoutAck()
    {
        // Arrange
        string userId = NewUserId();
        await using ConnectedClient client = await ConnectAsync(userId);
        PushRequest request = WebAppFactory.CreatePushRequest(userId);

        // Act: never acknowledge
        await factory.PushAsync(request);

        // Assert
        Delivery delivery = await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Failed, TimeSpan.FromSeconds(30));
        Assert.Equal(WebAppFactory.MaxAttempts, client.CountMessages(request.EventId));
        Assert.Contains("Not acknowledged", delivery.LastError, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Acknowledge_ShouldOnlyAffectTheCallersOwnDelivery()
    {
        // Arrange
        string recipientId = NewUserId();
        string otherUserId = NewUserId();
        await using ConnectedClient recipient = await ConnectAsync(recipientId);
        await using ConnectedClient otherUser = await ConnectAsync(otherUserId);
        PushRequest request = WebAppFactory.CreatePushRequest(recipientId);

        await factory.PushAsync(request);
        await recipient.WaitForMessageAsync(request.EventId);

        // Act
        await otherUser.Connection.InvokeAsync(nameof(NotificationsHub.Acknowledge), request.EventId, TestContext.Current.CancellationToken);

        // Assert
        Delivery delivery = Assert.Single(await factory.GetDeliveriesAsync(request.EventId));
        Assert.NotEqual(DeliveryStatus.Delivered, delivery.Status);
    }

    [Fact]
    public async Task Cleanup_ShouldExpireParkedDelivery_WhenPastExpiresAt()
    {
        // Arrange
        string userId = NewUserId();
        PushRequest request = WebAppFactory.CreatePushRequest(userId) with { ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(1) };
        await factory.PushAsync(request);
        await factory.WaitForDeliveryAsync(request.EventId, d => d.Status == DeliveryStatus.Parked);
        await Task.Delay(TimeSpan.FromSeconds(1.5), TestContext.Current.CancellationToken);

        DeliveryCleanupWorker cleanup = factory.Services.GetServices<IHostedService>().OfType<DeliveryCleanupWorker>().Single();

        // Act
        await cleanup.RunOnceAsync(TestContext.Current.CancellationToken);

        // Assert
        Delivery delivery = Assert.Single(await factory.GetDeliveriesAsync(request.EventId));
        Assert.Equal(DeliveryStatus.Expired, delivery.Status);
    }

    private static string NewUserId() => Guid.NewGuid().ToString("N");

    private async Task<ConnectedClient> ConnectAsync(string userId)
    {
        HubConnection connection = factory.CreateHubConnection(userId);
        ConnectedClient client = new(connection);
        await connection.StartAsync(TestContext.Current.CancellationToken);
        return client;
    }

    /// <summary>A hub connection that records every notification it receives.</summary>
    private sealed class ConnectedClient : IAsyncDisposable
    {
        private readonly ConcurrentQueue<NotificationMessage> _received = new();
        private readonly SemaphoreSlim _receivedSignal = new(0);

        public ConnectedClient(HubConnection connection)
        {
            Connection = connection;
            Connection.On<NotificationMessage>(NotificationsHub.ReceiveNotificationMethod, message =>
            {
                _received.Enqueue(message);
                _receivedSignal.Release();
            });
        }

        public HubConnection Connection { get; }

        public int CountMessages(string eventId) => _received.Count(m => m.EventId == eventId);

        public async Task<NotificationMessage> WaitForMessageAsync(string eventId, int count = 1)
        {
            using CancellationTokenSource timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
            timeoutSource.CancelAfter(ReceiveTimeout);

            while (CountMessages(eventId) < count)
            {
                await _receivedSignal.WaitAsync(timeoutSource.Token);
            }

            return _received.First(m => m.EventId == eventId);
        }

        public async ValueTask DisposeAsync()
        {
            await Connection.DisposeAsync();
            _receivedSignal.Dispose();
        }
    }
}
