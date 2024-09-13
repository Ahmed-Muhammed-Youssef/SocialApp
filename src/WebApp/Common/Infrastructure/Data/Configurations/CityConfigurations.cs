using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CityConfigurations : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            // Properties
            builder.Property(city => city.Name).IsRequired();
            builder.Property(city => city.AName).IsRequired();

            builder.HasMany<ApplicationUser>().WithOne().HasForeignKey(u => u.CityId);
        }
    }
}
