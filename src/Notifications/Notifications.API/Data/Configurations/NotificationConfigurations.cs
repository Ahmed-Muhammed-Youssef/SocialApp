namespace Notifications.API.Data.Configurations;

public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        // Key
        builder.HasKey(n => n.Id);

        // Properties
        builder.Property(n => n.EventId).IsRequired().HasMaxLength(128);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Property(n => n.Body).HasMaxLength(2000);
        builder.Property(n => n.Data).HasColumnType("jsonb");
        builder.Property(n => n.CreatedAt).IsRequired();

        // A duplicate event_id is how a repeated send from a producer is detected.
        builder.HasIndex(n => n.EventId).IsUnique();

        builder.HasMany(n => n.Recipients)
            .WithOne(r => r.Notification)
            .HasForeignKey(r => r.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
