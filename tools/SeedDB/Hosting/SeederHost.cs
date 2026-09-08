using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared.Constants;

namespace SeedDB.Hosting;

/// <summary>
/// Builds the seeder's service provider from the application's own registrations
/// (<see cref="DependencyInjection.AddInfrastructureServices"/>) rather than hand-wiring a
/// <c>DbContext</c>, so seeded data goes through the same repositories, caching decorator and
/// aggregates the API uses and cannot drift from them.
/// </summary>
internal static class SeederHost
{
    public static ServiceProvider CreateServiceProvider(SeedOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:AppConnection"] = options.ConnectionString,
        });

        builder.Services.AddLogging(logging =>
        {
            logging.AddSimpleConsole(console => console.SingleLine = true);

            // Seeding issues thousands of statements; logging each one buries the progress output.
            // Same filter the API host applies.
            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();

        builder.AddInfrastructureServices();

        // AddInfrastructureServices deliberately stops short of ASP.NET Identity — the API host
        // registers it. A seeder that creates login-capable users needs UserManager/RoleManager,
        // and the password policy must mirror API.DependencyInjection.AddIdentity or every
        // generated password is rejected by validation.
        builder.Services.AddIdentityCore<IdentityUser>(identity =>
            {
                identity.Password.RequireNonAlphanumeric = false;
                identity.SignIn.RequireConfirmedAccount = false;
                identity.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDatabaseContext>();

        // The HttpContext-backed implementation has no request to read from here. Seed stages act
        // on explicit user ids, so anything reaching for the "current user" is a bug worth surfacing.
        builder.Services.AddScoped<ICurrentUserService, UnavailableCurrentUserService>();

        return builder.Services.BuildServiceProvider();
    }

    /// <summary>
    /// Fails fast with an actionable message instead of letting the first stage crash mid-write.
    /// Seeding needs an existing schema plus the reference data every seeded user depends on:
    /// a city to live in and the roles assigned at creation.
    /// </summary>
    public static async Task EnsurePrerequisitesAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(provider);

        using IServiceScope scope = provider.CreateScope();
        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();

        if (!await context.Database.CanConnectAsync(cancellationToken))
        {
            throw new SeederPrerequisiteException(
                "Cannot reach the database given by --connection. Check the connection string, or run `docker compose up` to start the local SQL Server.");
        }

        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            throw new SeederPrerequisiteException(
                "The database has pending migrations. Run `dotnet ef database update --project src/API/Infrastructure --startup-project src/API/API` first.");
        }

        if (!await context.Cities.AnyAsync(cancellationToken))
        {
            throw new SeederPrerequisiteException(
                "No cities found — seeded users need a CityId. Start the API once to seed reference data, or apply scripts/seed_essential_data.sql.");
        }

        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(RolesNameValues.User))
        {
            throw new SeederPrerequisiteException(
                $"Role '{RolesNameValues.User}' does not exist. Start the API once to seed roles before seeding users.");
        }
    }
}
