using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Auth.GoogleSignIn;

public class GoogleSignInHandler(UserManager<IdentityUser> userManager, ITokenProvider tokenService, IUnitOfWork unitOfWork, IIdentityDbContext identityDbContext, IConfiguration configuration) : ICommandHandler<GoogleSignInCommand, Result<LoginDTO>>
{
    private readonly string googleClientId = configuration["Authentication:Google:ClientId"]!;
    public async ValueTask<Result<LoginDTO>> Handle(GoogleSignInCommand command, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(command.Credential,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [googleClientId],
                });
        }
        catch
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }


        if(!payload.EmailVerified)
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }


        UserLoginInfo loginInfo = new(
            loginProvider: LoginProviders.Google,
            providerKey: payload.Subject,
            providerDisplayName: LoginProviders.Google
        );

        IdentityUser? existingLoginUser = await userManager.FindByLoginAsync(LoginProviders.Google, payload.Subject);


        if (existingLoginUser is null)
        {
            IdentityUser? identityUser = await userManager.FindByEmailAsync(payload.Email);

            if (identityUser is null) // new user
            {
                identityUser = new()
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    EmailConfirmed = payload.EmailVerified
                };

                var result = await userManager.CreateAsync(identityUser);

                if (!result.Succeeded)
                {
                    return Result<LoginDTO>.Error("Failed to register the user.");
                }
                var adddRoleresult = await userManager.AddToRoleAsync(identityUser, RolesNameValues.User);
                if (!adddRoleresult.Succeeded)
                {
                    return Result<LoginDTO>.Error("Failed to assign role to the user.");
                }
            }
            else
            {
                if (await userManager.HasPasswordAsync(identityUser))
                {
                    return Result<LoginDTO>.Error("Please sign in with email/password and link Google manually.");
                }
            }

            await userManager.AddLoginAsync(identityUser, loginInfo);
            existingLoginUser = identityUser;
        }

        UserByIdentitySpecification spec = new(existingLoginUser.Id);

        ApplicationUser? appUser = await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (appUser is null)
        {
            appUser = new ApplicationUser(
                identityId: existingLoginUser.Id,
                firstName: payload.GivenName ?? string.Empty,
                lastName: payload.FamilyName ?? string.Empty,
                dateOfBirth: DateTime.UtcNow.AddYears(-20),
                gender: Gender.NotSpecified,
                cityId: 1);

            unitOfWork.ApplicationUserRepository.Add(appUser);
            await unitOfWork.CommitAsync(cancellationToken);
        }

        TokenRequest tokenRequest = new(
            UserId: appUser.Id.ToString(),
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
