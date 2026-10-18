namespace Notifications.API;

public static class DependencyInjection
{
    public const string ConnectionStringName = "NotificationsConnection";

    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured.");

        builder.Services.AddDbContext<NotificationsDbContext>(options => ConfigureDbContext(options, connectionString));
        return builder;
    }

    /// <summary>Shared by the app and <see cref="DesignTimeDbContextFactory"/> so migrations see the same model.</summary>
    public static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
    }
}
