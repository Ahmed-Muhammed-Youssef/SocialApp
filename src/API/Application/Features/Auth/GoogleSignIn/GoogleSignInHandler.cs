using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application.Features.Auth.GoogleSignIn;

public class GoogleSignInHandler(UserManager<IdentityUser> userManager, ITokenProvider tokenService, IUnitOfWork unitOfWork, IIdentityDbContext identityDbContext) : ICommandHandler<GoogleSignInCommand, Result<LoginDTO>>
{
    public async ValueTask<Result<LoginDTO>> Handle(GoogleSignInCommand command, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(command.Credential);
        }
        catch
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }

        if(!payload.EmailVerified)
        {
            return Result<LoginDTO>.Unauthorized("Invalid Google credential.");
        }

        string googleUserId = payload.Subject;

        UserLoginInfo loginInfo = new(
            loginProvider: "Google",
            providerKey: payload.Subject,
            providerDisplayName: "Google"
        );

        IdentityUser? identityUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == payload.Email, cancellationToken);

        if (identityUser is null)
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

            await userManager.AddLoginAsync(identityUser, loginInfo);
        }
        else
        {
            var identityWithLogin = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

            if(identityWithLogin is null || identityWithLogin.Id == identityUser.Id)
            {
                return Result<LoginDTO>.Error("A user with the same email already exists.");
            }
            else
            {
                await userManager.AddLoginAsync(identityUser, loginInfo);
            }
        }

        UserByIdentitySpecification spec = new(identityUser.Id);

        ApplicationUser? appUser = await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (appUser is null)
        {
            appUser = new ApplicationUser(
                identityId: identityUser.Id,
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
            UserEmail: identityUser.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(identityUser)
        );

        string accessToken = tokenService.CreateAccessToken(tokenRequest);
        Domain.AuthUserAggregate.RefreshToken refreshToken = tokenService.CreateRefreshToken(identityUser.Id);

        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        return Result<LoginDTO>.Success(new LoginDTO(UserMappings.ToDto(appUser), accessToken, refreshToken.Token, refreshToken.ExpiresAtUtc));
    }
}
