namespace Notifications.API.Data.Configurations;

public class InboxItemConfigurations : IEntityTypeConfiguration<InboxItem>
{
    public void Configure(EntityTypeBuilder<InboxItem> builder)
    {
        builder.ToTable("inbox_items");

        // Also serves the sync query: WHERE recipient_id = @r AND seq > @cursor ORDER BY seq.
        builder.HasKey(i => new { i.RecipientId, i.Seq });

        // properties
        builder.Property(i => i.RecipientId).HasMaxLength(64).IsRequired();
        builder.Property(i => i.Seq).ValueGeneratedNever().IsRequired();
        builder.Property(i => i.EventId).HasMaxLength(128).IsRequired();

        // relationships
        builder.HasOne(i => i.Notification)
            .WithMany()
            .IsRequired()
            .HasForeignKey(i => i.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
