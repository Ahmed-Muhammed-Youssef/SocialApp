using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
            .HasKey(m => new { m.UserId, m.MatchedId });
            modelBuilder.Entity<Like>()
            .HasKey(l => new { l.LikedId, l.LikerId });
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
