using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CityConfigurations : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(city => city.Id);

        // Properties
        builder.Property(city => city.Name).HasMaxLength(255).IsRequired();

        builder.HasMany<ApplicationUser>().WithOne().HasForeignKey(u => u.CityId);
    }
}
