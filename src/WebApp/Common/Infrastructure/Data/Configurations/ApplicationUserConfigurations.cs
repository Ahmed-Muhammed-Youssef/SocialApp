﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Sex).IsRequired();
            builder.Property(u => u.Interest).IsRequired();
            builder.Property(u => u.DateOfBirth).IsRequired();
            builder.Property(u => u.Created).IsRequired();
            builder.Property(u => u.LastActive).IsRequired();
            builder.Property(u => u.CountryId).IsRequired();
            builder.Property(u => u.CityId).IsRequired();

            // Nullable Fields 
            builder.Property(u => u.ProfilePictureUrl).IsRequired(false);
        }
    }
}
