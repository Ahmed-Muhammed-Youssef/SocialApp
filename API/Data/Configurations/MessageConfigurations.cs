using API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Configurations
{
    public class MessageConfigurations: IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            // properties
            builder.Property(m => m.Id).IsRequired();
            builder.Property(m => m.Content).IsRequired();
            builder.Property(m => m.SentDate).IsRequired();
            builder.Property(m => m.SenderDeleted).IsRequired();
            builder.Property(m => m.RecipientDeleted).IsRequired();

            // relationships
            // with appuser (sender)
            builder.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // with appuser (recipient)
            builder.HasOne(m => m.Recipient)
                .WithMany()
                .HasForeignKey(m => m.RecipientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
