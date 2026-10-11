namespace Notifications.API.Data.Configurations;

public class InboxCounterConfigurations : IEntityTypeConfiguration<InboxCounter>
{
    public void Configure(EntityTypeBuilder<InboxCounter> builder)
    {
        builder.ToTable("inbox_counters");
        builder.HasKey(c => c.RecipientId);

        // properties
        builder.Property(c => c.RecipientId).HasMaxLength(64).IsRequired();
        builder.Property(c => c.LastSeq).IsRequired();
    }
}
