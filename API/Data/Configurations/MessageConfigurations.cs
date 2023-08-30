using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class MessageConfigurations : IEntityTypeConfiguration<Message>
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
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // with appuser (recipient)
            builder.HasOne(m => m.Recipient)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.RecipientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
