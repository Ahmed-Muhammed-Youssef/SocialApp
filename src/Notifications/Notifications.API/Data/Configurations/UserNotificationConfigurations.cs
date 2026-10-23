namespace Notifications.API.Data.Configurations;

public class UserNotificationConfigurations : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.ToTable("user_notifications");

        // Key
        builder.HasKey(r => new { r.NotificationId, r.RecipientId });

        // Properties
        builder.Property(r => r.RecipientId).IsRequired().HasMaxLength(NotificationLimits.RecipientIdMaxLength);
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.ReadAt).IsRequired(false);

        // A user's list, newest first.
        builder.HasIndex(r => new { r.RecipientId, r.CreatedAt })
            .IsDescending(false, true);

        // A user's unread count.
        builder.HasIndex(r => r.RecipientId)
            .HasDatabaseName("ix_user_notifications_unread")
            .HasFilter("read_at IS NULL");
    }
}
