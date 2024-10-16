﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RegionConfigurations : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.Id).IsRequired().ValueGeneratedNever();
            builder.Property(r => r.Name).HasMaxLength(255).IsRequired();

            builder.HasMany<City>().WithOne().HasForeignKey(city => city.RegionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
