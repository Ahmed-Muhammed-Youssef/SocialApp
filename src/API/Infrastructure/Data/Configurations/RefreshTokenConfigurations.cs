namespace Infrastructure.Data.Configurations;

public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(rt => rt.Token)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
