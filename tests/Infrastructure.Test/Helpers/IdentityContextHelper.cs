using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Helpers;

public static class IdentityContextHelper
{
    public static ApplicationDatabaseContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDatabaseContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
