using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class FriendConfigurations : IEntityTypeConfiguration<Friend>
{
    public void Configure(EntityTypeBuilder<Friend> builder)
    {
        builder.HasKey(fr => new { fr.UserId, fr.FriendId });

        // properties
        builder.Property(f => f.UserId).IsRequired();
        builder.Property(f => f.FriendId).IsRequired();
        builder.Property(f => f.Created).IsRequired();

        // relationships
        // with app user (user)
        builder.HasOne(f => f.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // with app user (friend)
        builder.HasOne(f => f.FriendUser)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
