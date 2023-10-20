using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Configurations
{
    public class FriendRequestConfigurations : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(fr => new { fr.RequesterId, fr.RequestedId });

            // properties
            builder.Property(fr => fr.RequestedId).IsRequired();
            builder.Property(fr => fr.RequesterId).IsRequired();
            builder.Property(fr => fr.Date).IsRequired();

            // relationships
            // with app user (requester)
            builder.HasOne(fr => fr.Requester)
                .WithMany(u => u.FriendRequestsSent)
                .HasForeignKey(fr => fr.RequesterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            // with app user (requested)
            builder.HasOne(fr => fr.Requested)
                .WithMany(u => u.FriendRequestsReceived)
                .HasForeignKey(fr => fr.RequestedId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
