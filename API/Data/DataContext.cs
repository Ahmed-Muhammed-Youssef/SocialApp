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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
            .HasKey(m => new { m.UserId, m.MatchedId });
            modelBuilder.Entity<Like>()
            .HasKey(l => new { l.LikedId, l.LikerId });
        }
    }
}
