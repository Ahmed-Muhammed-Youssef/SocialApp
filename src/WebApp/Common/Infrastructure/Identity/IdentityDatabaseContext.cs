using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityDatabaseContext : IdentityDbContext
    {
        public IdentityDatabaseContext() { }
        public IdentityDatabaseContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppSettings.IdentityConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
