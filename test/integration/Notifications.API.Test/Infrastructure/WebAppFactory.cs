using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Notifications.API.Channels.SignalR;
using Notifications.API.Data;
using Notifications.API.Entities;
using Notifications.API.Features.Push;
using Testcontainers.PostgreSql;

namespace Notifications.API.Test.Infrastructure;

public sealed class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public const string ApiKey = "test-api-key";
    public const int MaxAttempts = 3;

    private const string JwtIssuer = "test-issuer";
    private const string JwtAudience = "test-audience";
    private const string JwtKey = "test-signing-key-that-is-long-enough-for-hmac-sha512-0123456789abcdef";

    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:17-alpine")
        .WithName($"NotificationsDb{Guid.NewGuid()}")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:NotificationsConnection", _postgresContainer.GetConnectionString());
        builder.UseSetting("Jwt:Issuer", JwtIssuer);
        builder.UseSetting("Jwt:Audience", JwtAudience);
        builder.UseSetting("Jwt:Key", JwtKey);
        builder.UseSetting("Intake:ApiKey", ApiKey);

        // Short timings so retry, ack-timeout and give-up paths complete within a test.
        builder.UseSetting("Delivery:Channels:signalr:PollInterval", "00:00:00.200");
        builder.UseSetting("Delivery:Channels:signalr:AckTimeout", "00:00:01");
        builder.UseSetting("Delivery:Channels:signalr:BackoffBase", "00:00:00.200");
        builder.UseSetting("Delivery:Channels:signalr:MaxAttempts", MaxAttempts.ToString(System.Globalization.CultureInfo.InvariantCulture));

        builder.UseEnvironment("Development"); // applies migrations at startup

        base.ConfigureWebHost(builder);
    }

    public async ValueTask InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await base.DisposeAsync();
    }

    /// <summary>Creates a token shaped like the main API's (user id in the <c>nameid</c> claim).</summary>
    public static string CreateAccessToken(string userId)
    {
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = JwtIssuer,
            Audience = JwtAudience,
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId)]),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)), SecurityAlgorithms.HmacSha512Signature),
        };

        JwtSecurityTokenHandler handler = new();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    /// <param name="userId">The connecting user, or <see langword="null"/> to connect without a token.</param>
    public HubConnection CreateHubConnection(string? userId)
    {
        TestServer server = Server;
        return new HubConnectionBuilder()
            .WithUrl(new Uri(server.BaseAddress, NotificationsHub.Route), options =>
            {
                options.HttpMessageHandlerFactory = _ => server.CreateHandler();
                options.Transports = HttpTransportType.LongPolling;
                if (userId is not null)
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(CreateAccessToken(userId));
                }
            })
            .Build();
    }

    public async Task<HttpResponseMessage> PushAsync(PushRequest request, string? apiKey = ApiKey)
    {
        HttpClient client = CreateClient();
        using HttpRequestMessage message = new(HttpMethod.Post, PushEndpoint.Route);
        message.Content = JsonContent.Create(request);
        if (apiKey is not null)
        {
            message.Headers.Add(ApiKeyEndpointFilter.HeaderName, apiKey);
        }

        return await client.SendAsync(message, TestContext.Current.CancellationToken);
    }

    public async Task<List<Delivery>> GetDeliveriesAsync(string eventId)
    {
        using IServiceScope scope = Services.CreateScope();
        NotificationsDbContext db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
        return await db.Deliveries.AsNoTracking()
            .Where(d => d.EventId == eventId)
            .OrderBy(d => d.RecipientId)
            .ToListAsync(TestContext.Current.CancellationToken);
    }

    /// <summary>Polls until the single delivery of <paramref name="eventId"/> satisfies <paramref name="condition"/>.</summary>
    public async Task<Delivery> WaitForDeliveryAsync(string eventId, Func<Delivery, bool> condition, TimeSpan? timeout = null)
    {
        using CancellationTokenSource timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        timeoutSource.CancelAfter(timeout ?? TimeSpan.FromSeconds(15));

        Delivery? last = null;
        while (!timeoutSource.IsCancellationRequested)
        {
            last = (await GetDeliveriesAsync(eventId)).SingleOrDefault();
            if (last is not null && condition(last))
            {
                return last;
            }

            try
            {
                await Task.Delay(100, timeoutSource.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        throw new TimeoutException($"Delivery of {eventId} didn't reach the expected state. Last seen: {last?.Status.ToString() ?? "none"}.");
    }

    public static PushRequest CreatePushRequest(params string[] recipients) => new()
    {
        EventId = Guid.NewGuid().ToString(),
        Title = "New friend request",
        Body = "John Doe sent you a friend request",
        Recipients = recipients,
        Data = new System.Text.Json.Nodes.JsonObject { ["requesterId"] = 42 },
    };
}
