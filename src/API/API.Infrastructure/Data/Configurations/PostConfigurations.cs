﻿using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Configurations
{
    public class PostConfigurations : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            // properties
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.Content).IsRequired();
            builder.Property(p => p.DatePosted).IsRequired();
            builder.Property(p => p.DateEdited).IsRequired(false);

            // relationships
            builder.HasOne(p => p.AppUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
