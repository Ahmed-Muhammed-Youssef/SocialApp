namespace Infrastructure.Data.Configurations;

public class IdentityUserConfigurations : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
