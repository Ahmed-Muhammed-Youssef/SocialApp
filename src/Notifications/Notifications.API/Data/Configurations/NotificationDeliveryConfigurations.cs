namespace Notifications.API.Data.Configurations;

public class NotificationDeliveryConfigurations : IEntityTypeConfiguration<NotificationDelivery>
{
    public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
    {
        // Only a Pending delivery has no ProcessedAt; every final status records when it was reached.
        builder.ToTable("notification_deliveries", t => t.HasCheckConstraint(
            "ck_notification_deliveries_processed_at_matches_status",
            "(status = 'Pending') = (processed_at IS NULL)"));

        // Key
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.Channel).IsRequired().HasMaxLength(NotificationLimits.ChannelMaxLength);
        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(NotificationLimits.DeliveryStatusMaxLength);
        builder.Property(d => d.CreatedAt).IsRequired();
        builder.Property(d => d.Attempts).IsRequired();
        builder.Property(d => d.NextAttemptAt).IsRequired();
        builder.Property(d => d.LastError).HasMaxLength(NotificationLimits.DeliveryErrorMaxLength);

        // A notification is queued at most once per channel.
        builder.HasIndex(d => new { d.NotificationId, d.Channel }).IsUnique();

        builder.HasOne(d => d.Notification)
            .WithMany()
            .HasForeignKey(d => d.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
