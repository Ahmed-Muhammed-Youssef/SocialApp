using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Key
            builder.HasKey(u => u.Id);

            // properties
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(255);
            builder.Property(u => u.ProfilePictureUrl).IsRequired(false);
            builder.Property(u => u.Sex).IsRequired();
            builder.Property(u => u.Interest).IsRequired();
            builder.Property(u => u.DateOfBirth).IsRequired();
            builder.Property(u => u.Created).IsRequired();
            builder.Property(u => u.LastActive).IsRequired();
            builder.Property(u => u.City).IsRequired();
            builder.Property(u => u.Country).IsRequired();

            // relationships
            // with user roles
            builder.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
