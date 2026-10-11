namespace Notifications.API.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        ILogger logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(DatabaseInitializer).FullName ?? nameof(DatabaseInitializer));

        // Production applies migrations explicitly (dotnet ef database update), never at startup.
        if (!services.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            return;
        }

        try
        {
            await services.GetRequiredService<NotificationsDbContext>().Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}
