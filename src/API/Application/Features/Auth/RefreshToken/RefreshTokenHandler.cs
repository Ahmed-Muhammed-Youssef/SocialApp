namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenHandler(ITokenProvider tokenProvider, ICurrentUserService currentUserService, IIdentityDbContext identityDbContext, UserManager<IdentityUser> userManager) : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
{
    public async ValueTask<Result<RefreshTokenResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        Domain.AuthUserAggregate.RefreshToken? refreshToken = await identityDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == command.RefreshToken && !rt.IsExpired, cancellationToken);

        if (refreshToken is null || refreshToken.User is null)
        {
            return Result<RefreshTokenResult>.Unauthorized("Invalid or expired refresh token.");
        }

        var userPublicId = currentUserService.GetPublicId();

        // Generate new access token and refresh token

        string newRefreshToken = await tokenProvider.CreateRefreshToken(refreshToken.UserId);

        TokenRequest tokenRequest = new(
            UserId: userPublicId.ToString(),
            UserEmail: refreshToken.User.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(refreshToken.User)
        );

        string accessToken = tokenProvider.CreateAccessToken(tokenRequest);

        // @ToDo: Invalidate the old refresh token 
        // @ToDo: expiry date should be the persisted refresh token expiry date

        RefreshTokenResult refreshTokenDTO = new(accessToken, newRefreshToken, DateTime.UtcNow.AddDays(7));

        return Result<RefreshTokenResult>.Success(refreshTokenDTO);
    }
}
