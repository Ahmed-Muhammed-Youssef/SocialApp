using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : IdentityDbContext(options)
{
}
