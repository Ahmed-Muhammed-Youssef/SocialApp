using Application.Features.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using WireMock.Server;

namespace API.Test.Infrastructure;

public sealed class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
        .WithName($"AppDb{Guid.NewGuid()}")
        .WithPassword("Password123!")
        .Build();

    private WireMockServer _wireMockServer;

    public WireMockServer GetWireMockServer() => _wireMockServer;
    private readonly MockGoogleCredentialValidator _mockGoogleValidator = new();

    public MockGoogleCredentialValidator GetMockGoogleValidator() => _mockGoogleValidator;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:AppConnection", _sqlContainer.GetConnectionString());
        builder.UseSetting("Authentication:Google:ApiEndpoint", _wireMockServer.Urls[0]);
        builder.UseSetting("Authentication:Google:TokenEndpoint", _wireMockServer.Urls[0]);
        builder.UseSetting("Authentication:Google:ClientId", "test-client-id"); // Test client ID for Google Sign In tests
        builder.UseEnvironment("Development"); // it's the default value, but we added it to be more explicit and to avoid any confusion when debugging tests

        // Register mock Google credential validator for testing
        builder.ConfigureServices(services =>
        {
            // Remove the production Google credential validator
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IGoogleCredentialValidator));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the mock validator for tests (as singleton so we can control it from tests)
            services.AddSingleton<IGoogleCredentialValidator>(_mockGoogleValidator);
        });

        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        _wireMockServer = WireMockServer.Start();
    }

    public new async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
        _wireMockServer.Stop();
    }
}
