namespace Infrastructure.Data.Identity;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : IdentityDbContext(options)
{
}
