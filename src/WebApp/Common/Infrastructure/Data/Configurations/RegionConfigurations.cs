namespace Infrastructure.Data.Configurations;

public class RegionConfigurations : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Name).HasMaxLength(255).IsRequired();

        builder.HasMany(r => r.Cities)
            .WithOne(c => c.Region)
            .HasForeignKey(city => city.RegionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
