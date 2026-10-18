namespace Notifications.API.Data;

public static class DatabaseInitializer
{
    /// <summary>
    /// Applies migrations in Development only. Other environments migrate before deploying,
    /// so several instances never race to migrate at startup.
    /// </summary>
    public static async Task InitializeAsync(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
        NotificationsDbContext dbContext = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
