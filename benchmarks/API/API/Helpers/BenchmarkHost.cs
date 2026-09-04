using System.Globalization;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Benchmark.Helpers;

/// <summary>
/// Builds the benchmark's service provider from the application's own DI registrations
/// (<see cref="DependencyInjection.AddInfrastructureServices"/>) rather than hand-wiring repositories.
/// A change to how the <c>DbContext</c>, the repositories or the caching decorator are composed therefore
/// flows into the benchmarks automatically instead of leaving them silently measuring the wrong thing.
/// </summary>
public static class BenchmarkHost
{
    /// <summary>Environment variable holding the SQL Server connection string to benchmark against.</summary>
    public const string ConnectionStringVariable = "SOCIALAPP_BENCHMARK_CONNECTION";

    /// <summary>Environment variable holding the id of the seeded user the benchmarks impersonate.</summary>
    public const string UserIdVariable = "SOCIALAPP_BENCHMARK_USER_ID";

    /// <summary>Environment variable holding the id of a seeded post used by single-post reads.</summary>
    public const string PostIdVariable = "SOCIALAPP_BENCHMARK_POST_ID";

    /// <summary>Matches the SQL Server that <c>docker compose up</c> exposes on localhost.</summary>
    private const string FallbackConnectionString =
        "Server=localhost,1433;Database=AppDb;User Id=sa;Password=Password123!;TrustServerCertificate=True;";

    /// <summary>The user the benchmarks run as. Must exist, and should have friends and posts to be meaningful.</summary>
    public static int UserId { get; } = ReadInt32(UserIdVariable, 1);

    /// <summary>The post fetched by single-post reads. Must exist.</summary>
    public static ulong PostId { get; } = ReadUInt64(PostIdVariable, 1);

    public static ServiceProvider CreateServiceProvider()
    {
        HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:AppConnection"] = ReadConnectionString(),
        });

        builder.Services.AddLogging();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();

        builder.AddInfrastructureServices();

        // Registered after AddInfrastructureServices so it replaces the HttpContext-backed implementation.
        builder.Services.AddScoped<ICurrentUserService>(_ =>
            new BenchmarkCurrentUserService(UserId, "benchmark@socialapp.local", ["user"]));

        return builder.Services.BuildServiceProvider();
    }

    /// <summary>
    /// Fails fast with an actionable message rather than letting every iteration throw a connection error,
    /// which BenchmarkDotNet would otherwise report as an opaque "benchmark failed to run".
    /// </summary>
    public static async Task VerifyDatabaseAsync(IServiceProvider provider)
    {
        using IServiceScope scope = provider.CreateScope();
        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();

        if (!await context.Database.CanConnectAsync())
        {
            throw new InvalidOperationException(
                $"Cannot reach the benchmark database. Set {ConnectionStringVariable}, or run `docker compose up` " +
                "to start the local SQL Server this falls back to.");
        }

        if (!await context.ApplicationUsers.AnyAsync(u => u.Id == UserId))
        {
            throw new InvalidOperationException(
                $"User {UserId} does not exist in the benchmark database. Seed it, or point {UserIdVariable} at a user that does. " +
                "Benchmarks against an empty database measure nothing useful.");
        }
    }

    /// <summary>
    /// Empties the shared <see cref="IMemoryCache"/> that <c>CachedUserRepository</c> reads through.
    /// Without this the first iteration populates the cache and every later one measures a dictionary
    /// lookup instead of a query — the numbers would look excellent and mean nothing.
    /// </summary>
    public static void ClearCache(IServiceProvider provider)
    {
        if (provider.GetRequiredService<IMemoryCache>() is MemoryCache cache)
        {
            cache.Clear();
        }
    }

    private static string ReadConnectionString()
        => Environment.GetEnvironmentVariable(ConnectionStringVariable) is { Length: > 0 } value
            ? value
            : FallbackConnectionString;

    private static int ReadInt32(string variable, int fallback)
        => int.TryParse(Environment.GetEnvironmentVariable(variable), CultureInfo.InvariantCulture, out int value)
            ? value
            : fallback;

    private static ulong ReadUInt64(string variable, ulong fallback)
        => ulong.TryParse(Environment.GetEnvironmentVariable(variable), CultureInfo.InvariantCulture, out ulong value)
            ? value
            : fallback;
}
