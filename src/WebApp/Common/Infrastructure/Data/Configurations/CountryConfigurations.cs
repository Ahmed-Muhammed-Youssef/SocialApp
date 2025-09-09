using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CountryConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(country => country.Id);

            // Properties
            builder.Property(country => country.Name).HasMaxLength(255).IsRequired();
            builder.Property(country => country.Code).HasMaxLength(2).IsRequired();
            builder.Property(country => country.Language).HasMaxLength(3).IsRequired();

            builder.HasMany(c => c.Regions)
                .WithOne()
                .HasForeignKey(region => region.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
