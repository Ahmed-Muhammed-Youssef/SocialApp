using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Entities;
using Notifications.API.Features.Push;
using Notifications.API.Test.Infrastructure;

namespace Notifications.API.Test.Features;

public sealed class PushTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    [Fact]
    public async Task Push_ShouldReturn202_AndCreateDeliveryPerRecipient()
    {
        // Arrange
        PushRequest request = WebAppFactory.CreatePushRequest("user-a", "user-b", "user-a");

        // Act
        HttpResponseMessage response = await factory.PushAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        PushResponse? body = await response.Content.ReadFromJsonAsync<PushResponse>(TestContext.Current.CancellationToken);
        Assert.Equal(new PushResponse(request.EventId, Duplicate: false), body);

        List<Delivery> deliveries = await factory.GetDeliveriesAsync(request.EventId);
        Assert.Equal(["user-a", "user-b"], deliveries.Select(d => d.RecipientId));
        Assert.All(deliveries, d => Assert.Equal("signalr", d.Channel));
    }

    [Fact]
    public async Task Push_ShouldBeIdempotent_WhenSameEventIdPostedTwice()
    {
        // Arrange
        PushRequest request = WebAppFactory.CreatePushRequest("user-a");
        await factory.PushAsync(request);

        // Act
        HttpResponseMessage response = await factory.PushAsync(request with { Title = "Different title" });

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        PushResponse? body = await response.Content.ReadFromJsonAsync<PushResponse>(TestContext.Current.CancellationToken);
        Assert.True(body?.Duplicate);
        Assert.Single(await factory.GetDeliveriesAsync(request.EventId));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("wrong-key")]
    public async Task Push_ShouldReturn401_WithoutValidApiKey(string? apiKey)
    {
        // Arrange
        PushRequest request = WebAppFactory.CreatePushRequest("user-a");

        // Act
        HttpResponseMessage response = await factory.PushAsync(request, apiKey);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Empty(await factory.GetDeliveriesAsync(request.EventId));
    }

    [Fact]
    public async Task Push_ShouldReturn400_WithInvalidRequest()
    {
        // Arrange
        PushRequest request = WebAppFactory.CreatePushRequest() with
        {
            Title = "",
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-1),
        };

        // Act
        HttpResponseMessage response = await factory.PushAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
        Assert.NotNull(problem);
        Assert.Contains(nameof(PushRequest.Title), problem.Errors.Keys);
        Assert.Contains(nameof(PushRequest.Recipients), problem.Errors.Keys);
        Assert.Contains(nameof(PushRequest.ExpiresAt), problem.Errors.Keys);
    }
}
