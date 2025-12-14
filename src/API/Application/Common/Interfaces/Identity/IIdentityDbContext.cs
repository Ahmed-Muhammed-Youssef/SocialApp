namespace Application.Common.Interfaces.Identity;

public interface IIdentityDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
