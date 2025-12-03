using Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Helpers;

public static class IdentityContextHelper
{
    public static IdentityDatabaseContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<IdentityDatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new IdentityDatabaseContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
