namespace Notifications.API.Data.Configurations;

public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.EventId);

        // properties
        builder.Property(n => n.EventId).HasMaxLength(128).IsRequired();
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Body).HasMaxLength(2000).IsRequired(false);
        builder.Property(n => n.Data).HasColumnType("jsonb").IsRequired(false);
        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.ExpiresAt).IsRequired(false);
        builder.Property(n => n.ReceivedAt).IsRequired();

        // relationships
        builder.HasMany(n => n.Deliveries)
            .WithOne(d => d.Notification)
            .IsRequired()
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
