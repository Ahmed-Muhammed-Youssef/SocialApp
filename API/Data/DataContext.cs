using API.Data.Configurations;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext <AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>
        , IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations
            modelBuilder.ApplyConfiguration(new PictureConfigurations());
            modelBuilder.ApplyConfiguration(new MessageConfigurations());
            modelBuilder.ApplyConfiguration(new GroupConfigurations());
            modelBuilder.ApplyConfiguration(new FriendRequestConfigurations());
            modelBuilder.ApplyConfiguration(new FriendConfigurations());

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            modelBuilder.Entity<AppRole>()
               .HasMany(u => u.UserRoles)
               .WithOne(ur => ur.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
