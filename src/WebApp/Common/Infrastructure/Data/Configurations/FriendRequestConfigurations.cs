namespace Infrastructure.Data.Configurations;

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
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(fr => fr.RequesterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        // with app user (requested)
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(fr => fr.RequestedId)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
