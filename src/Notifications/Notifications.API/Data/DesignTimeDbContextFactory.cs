using Microsoft.EntityFrameworkCore.Design;

namespace Notifications.API.Data;

/// <summary>
/// Lets <c>dotnet ef</c> build the context without running the app (and without its secrets).
/// Commands that touch a real database take <c>--connection</c>.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<NotificationsDbContext>();
        DependencyInjection.ConfigureDbContext(options, "Host=localhost;Database=notifications");
        return new NotificationsDbContext(options.Options);
    }
}
