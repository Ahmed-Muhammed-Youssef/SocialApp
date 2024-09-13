using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CountryConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Properties
            builder.Property(country => country.Name).IsRequired();
            builder.Property(country => country.AName).IsRequired();

            builder.HasMany<ApplicationUser>().WithOne().HasForeignKey(u => u.CountryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany<City>().WithOne().HasForeignKey(city => city.CountryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
