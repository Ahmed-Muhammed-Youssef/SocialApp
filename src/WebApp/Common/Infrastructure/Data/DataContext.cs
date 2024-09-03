using Infrastructure.Data.Configurations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppSettings.DefaultConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations
            modelBuilder.ApplyConfiguration(new PictureConfigurations());
            modelBuilder.ApplyConfiguration(new MessageConfigurations());
            modelBuilder.ApplyConfiguration(new GroupConfigurations());
            modelBuilder.ApplyConfiguration(new FriendRequestConfigurations());
            modelBuilder.ApplyConfiguration(new FriendConfigurations());
            modelBuilder.ApplyConfiguration(new AppUserConfigurations());
            modelBuilder.ApplyConfiguration(new PostConfigurations());
        }
    }
}
