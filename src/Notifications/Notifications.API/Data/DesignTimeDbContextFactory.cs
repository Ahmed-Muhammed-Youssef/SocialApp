using Microsoft.EntityFrameworkCore.Design;

namespace Notifications.API.Data;

/// <summary>
/// Lets <c>dotnet ef migrations add</c> build the model without starting the host (which requires JWT and API-key configuration).
/// The connection string is a placeholder: adding a migration never connects.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<NotificationsDbContext> options = new();
        DependencyInjection.ConfigureDbContext(options, "Host=localhost;Database=notifications");
        return new NotificationsDbContext(options.Options);
    }
}
