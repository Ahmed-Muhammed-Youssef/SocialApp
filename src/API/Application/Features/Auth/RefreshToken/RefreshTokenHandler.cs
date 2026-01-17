namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenHandler(ITokenProvider tokenProvider, IApplicationDatabaseContext identityDbContext, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork) : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
{
    // @TODO: Add Session-wide revocation on breach

    public async ValueTask<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        using var tx = await identityDbContext.Database.BeginTransactionAsync(cancellationToken);

        int rowsAffected = await identityDbContext.RefreshTokens
            .Where(rt => rt.Token == command.RefreshToken && rt.ExpiresAtUtc > DateTime.UtcNow)
            .ExecuteUpdateAsync(setters => setters.SetProperty(rt => rt.ExpiresAtUtc, DateTime.UtcNow), cancellationToken: cancellationToken);

        if (rowsAffected == 0)
        {
            return Result<RefreshTokenResult>.Unauthorized("Failed to invalidate the refresh token.");
        }

        Domain.AuthUserAggregate.RefreshToken refreshToken = await identityDbContext.RefreshTokens
            .Include(rt => rt.User)
            .SingleAsync(rt => rt.Token == command.RefreshToken, cancellationToken);

        if (refreshToken.User is null)
        {
            return Result<RefreshTokenResult>.Unauthorized("Invalid or expired refresh token.");
        }

        UserByIdentitySpecification spec = new(refreshToken.UserId);

        ApplicationUser? user = await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (user is null)
        {
            return Result<RefreshTokenResult>.Unauthorized("User associated with the refresh token not found.");
        }

        int userPublicId = user.Id;

        // Generate new access token and refresh token

        Domain.AuthUserAggregate.RefreshToken newRefreshToken = tokenProvider.CreateRefreshToken(refreshToken.UserId);

        identityDbContext.RefreshTokens.Add(newRefreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        await tx.CommitAsync(cancellationToken);

        TokenRequest tokenRequest = new(
            UserId: userPublicId.ToString(),
            UserEmail: refreshToken.User.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(refreshToken.User)
        );

        string accessToken = tokenProvider.CreateAccessToken(tokenRequest);

        RefreshTokenResult refreshTokenDTO = new(accessToken, newRefreshToken.Token, newRefreshToken.ExpiresAtUtc);

        return Result<RefreshTokenResult>.Success(refreshTokenDTO);
    }
}
