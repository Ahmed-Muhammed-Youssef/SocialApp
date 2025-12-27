namespace Application.Features.Auth.Login;

public class LoginHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenProvider tokenService, IApplicationDatabaseContext identityDbContext)
    : ICommandHandler<LoginCommand, Result<LoginDTO>>
{
    public async ValueTask<Result<LoginDTO>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        IdentityUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken: cancellationToken);

        if (user == null)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        SignInResult signInResult = await signInManager
            .CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        var userDtoByIdentitySpec = new UserDtoByIdentitySpecification(user.Id);

        UserDTO? userData = await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(userDtoByIdentitySpec, cancellationToken);

        if(userData is null)
        {
            return Result<LoginDTO>.Unauthorized();
        }

        TokenRequest tokenRequest = new(
            UserId: userData.Id.ToString(),
            UserEmail: user.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(user)
        );

        string accessToken = tokenService.CreateAccessToken(tokenRequest);
        Domain.AuthUserAggregate.RefreshToken refreshToken = tokenService.CreateRefreshToken(user.Id);

        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        return Result<LoginDTO>.Success(new LoginDTO(userData, accessToken, refreshToken.Token, refreshToken.ExpiresAtUtc));
    }
}
