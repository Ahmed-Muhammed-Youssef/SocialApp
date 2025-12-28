using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Test.Helpers;

public static class UserManagerTestHelper
{
    /// <summary>
    /// Creates a fully functional UserManager backed by a real EF Core InMemory database.
    /// 
    /// This factory supports:
    /// - IQueryable-based queries (AnyAsync, FirstOrDefaultAsync, etc.)
    /// - User creation and deletion
    /// - Role creation and assignment
    /// - Email uniqueness checks
    /// 
    /// This approach avoids mocking EF Core LINQ extension methods and is suitable
    /// for integration-style unit tests that exercise Identity behavior.
    /// </summary>
    /// <param name="users">
    /// Optional initial users to seed into the in-memory database.
    /// </param>
    /// <param name="roles">
    /// Optional initial roles to seed into the in-memory database.
    /// </param>
    public static UserManager<IdentityUser> CreateUserManagerWithUsers(List<IdentityUser>? users = null, List<IdentityRole>? roles = null)
    {
        users ??= [];
        roles ??= [];

        // Create an in-memory database context
        var options = new DbContextOptionsBuilder<TestIdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var context = new TestIdentityDbContext(options);
        if (users.Count > 0)
        {
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (roles.Count > 0)
        {
            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        // Create UserManager with minimal configuration for testing
        var identityOptions = Options.Create(new IdentityOptions
        {
            Password = new PasswordOptions
            {
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonAlphanumeric = false,
                RequiredLength = 1
            },
            User = new UserOptions
            {
                RequireUniqueEmail = true
            },
            SignIn = new SignInOptions
            {
                RequireConfirmedAccount = false
            }
        });
        
        var store = new UserStore<IdentityUser>(context);
        var userManager = new UserManager<IdentityUser>(
            store,
            identityOptions,
            new PasswordHasher<IdentityUser>(),
            [],
            [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!, // services
            null!  // logger
        );
        
        return userManager;
    }
    
    private class TestIdentityDbContext(DbContextOptions<UserManagerTestHelper.TestIdentityDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
    }
}

