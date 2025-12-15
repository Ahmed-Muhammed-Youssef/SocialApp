using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces.Identity;

public interface IIdentityDbContext
{
    DatabaseFacade Database {  get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
