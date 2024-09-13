﻿using Infrastructure.Data.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DataContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppSettings.DefaultConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
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
            modelBuilder.ApplyConfiguration(new ApplicationUserConfigurations());
            modelBuilder.ApplyConfiguration(new PostConfigurations());
            modelBuilder.ApplyConfiguration(new CountryConfigurations());
            modelBuilder.ApplyConfiguration(new CityConfigurations());
        }
    }
}
