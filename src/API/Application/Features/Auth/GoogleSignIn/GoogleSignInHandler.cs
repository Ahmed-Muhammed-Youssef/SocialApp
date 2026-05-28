using System.Globalization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Auth.GoogleSignIn;

public class GoogleSignInHandler(UserManager<IdentityUser> userManager, IUserProvisioningService userProvisioninService, ITokenProvider tokenService, IUnitOfWork unitOfWork, IApplicationDatabaseContext identityDbContext, IGoogleCredentialValidator credentialValidator) : ICommandHandler<GoogleSignInCommand, Result<LoginDTO>>
{
    public async ValueTask<Result<LoginDTO>> Handle(GoogleSignInCommand command, CancellationToken cancellationToken)
    {
        Google.Apis.Auth.GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await credentialValidator.ValidateAsync(command.Credential);
        }
        catch
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }


        if (!payload.EmailVerified)
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }


        IdentityUser? existingLoginUser = await userManager.FindByLoginAsync(LoginProviders.Google, payload.Subject);
        ApplicationUser? appUser = null;

        if (existingLoginUser is null)
        {
            IdentityUser? identityUser = await userManager.FindByEmailAsync(payload.Email);

            if (identityUser is null) // new user
            {
                UserProvisioningResult result = await userProvisioninService.CreateUserAsync(email: payload.Email, firstName: payload.GivenName ?? string.Empty, lastName: payload.FamilyName ?? string.Empty, cancellationToken);

                identityUser = result.IdentityUser;

                appUser = result.ApplicationUser;
            }
            else
            {
                if (await userManager.HasPasswordAsync(identityUser))
                {
                    return Result<LoginDTO>.Error("Please sign in with email/password and link Google manually.");
                }
            }

            UserLoginInfo loginInfo = new(
                loginProvider: LoginProviders.Google,
                providerKey: payload.Subject,
                providerDisplayName: LoginProviders.Google
            );

            await userManager.AddLoginAsync(identityUser, loginInfo);
            existingLoginUser = identityUser;
        }

        UserByIdentitySpecification spec = new(existingLoginUser.Id);

        appUser ??= await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (appUser is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        TokenRequest tokenRequest = new(
            UserId: appUser.Id.ToString(CultureInfo.InvariantCulture),
            UserEmail: existingLoginUser.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(existingLoginUser)
        );

        string accessToken = tokenService.CreateAccessToken(tokenRequest);
        Domain.AuthUserAggregate.RefreshToken refreshToken = tokenService.CreateRefreshToken(existingLoginUser.Id);

        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        return Result<LoginDTO>.Success(new LoginDTO(UserMappings.ToDto(appUser), accessToken, refreshToken.Token, refreshToken.ExpiresAtUtc));
    }
}
