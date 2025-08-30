using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityDatabaseContext : IdentityDbContext
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : base(options)
        {
        }
    }
}
