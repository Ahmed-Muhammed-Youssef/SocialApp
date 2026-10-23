using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Notifications.API.Test.Infrastructure;

public sealed class NotificationsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer = new PostgreSqlBuilder("postgres:17-alpine")
        .WithName($"NotificationsDb-{Guid.NewGuid():N}")
        .Build();

    /// <summary>Symmetric key used by both the factory (to configure JWT validation) and the test fixture (to mint tokens).</summary>
    public const string TestSigningKey = "integration-test-signing-key-that-is-long-enough-for-hs256!!";
    public const string TestIssuer = "notifications-test-issuer";
    public const string TestAudience = "notifications-test-audience";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:NotificationsConnection", _pgContainer.GetConnectionString());
        builder.UseSetting("Jwt:Key", TestSigningKey);
        builder.UseSetting("Jwt:Issuer", TestIssuer);
        builder.UseSetting("Jwt:Audience", TestAudience);
        builder.UseEnvironment("Development");
    }

    public async ValueTask InitializeAsync()
    {
        await _pgContainer.StartAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await _pgContainer.StopAsync();
        await base.DisposeAsync();
    }
}
