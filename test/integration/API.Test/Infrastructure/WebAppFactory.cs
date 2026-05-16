using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace API.Test.Infrastructure;

public sealed class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
        .WithName($"AppDb{Guid.NewGuid()}")
        .WithPassword("Password123!")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:AppConnection", _sqlContainer.GetConnectionString());
        builder.UseEnvironment("Development"); // it's the default value, but we added it to be more explicit and to avoid any confusion when debugging tests

        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
    }
}
