namespace Notifications.API.Data;

public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<InboxCounter> InboxCounters => Set<InboxCounter>();
    public DbSet<InboxItem> InboxItems => Set<InboxItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationsDbContext).Assembly);
    }
}
