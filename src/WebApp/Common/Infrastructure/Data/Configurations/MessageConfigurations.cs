using Domain.ChatAggregate;

namespace Infrastructure.Data.Configurations;

public class MessageConfigurations : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        // properties
        builder.Property(m => m.Id).IsRequired();
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.SentDate).IsRequired();

        // relationships
        // with appuser (sender)
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
