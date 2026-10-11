namespace Notifications.API.Data.Configurations;

public class DeliveryConfigurations : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");
        builder.HasKey(d => d.Id);

        // properties
        builder.Property(d => d.Id).UseIdentityAlwaysColumn();
        builder.Property(d => d.EventId).HasMaxLength(128).IsRequired();
        builder.Property(d => d.RecipientId).HasMaxLength(64).IsRequired();
        builder.Property(d => d.Channel).HasMaxLength(32).IsRequired();
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(16).IsRequired();
        builder.Property(d => d.Attempts).IsRequired();
        builder.Property(d => d.NextAttemptAt).IsRequired();
        builder.Property(d => d.LastError).HasMaxLength(2000).IsRequired(false);
        builder.Property(d => d.DeliveredAt).IsRequired(false);
        builder.Property(d => d.CreatedAt).IsRequired();

        // indexes
        builder.HasIndex(d => new { d.EventId, d.RecipientId, d.Channel }).IsUnique();

        // Serves the worker's claim query: due rows per channel.
        builder.HasIndex(d => new { d.Channel, d.NextAttemptAt })
            .HasDatabaseName("ix_deliveries_due")
            .HasFilter("status IN ('Pending', 'Processing', 'AwaitingAck')");

        // Serves the hub's un-park on reconnect.
        builder.HasIndex(d => new { d.RecipientId, d.Channel })
            .HasDatabaseName("ix_deliveries_parked")
            .HasFilter("status = 'Parked'");
    }
}
