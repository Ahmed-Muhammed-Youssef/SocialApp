namespace Infrastructure.Data.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Gender).IsRequired();
        builder.Property(u => u.DateOfBirth).IsRequired();
        builder.Property(u => u.Created).IsRequired();
        builder.Property(u => u.LastActive).IsRequired();
        builder.Property(u => u.CityId).IsRequired();
        builder.Property(u => u.IdentityId).HasMaxLength(450).IsRequired();
        // Nullable Fields 
        builder.Property(u => u.ProfilePictureId).IsRequired(false);
    }
}
