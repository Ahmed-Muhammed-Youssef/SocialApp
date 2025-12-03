namespace Application.Features.Auth.GoogleSignIn;

public class GoogleSignInHandler(IGoogleAuthService googleAuthService, UserManager<IdentityUser> userManager, ITokenProvider tokenService, PasswordGenerationService passwordGenerationService, IUnitOfWork unitOfWork) : ICommandHandler<GoogleSignInCommand, Result<LoginDTO>>
{
    public async ValueTask<Result<LoginDTO>> Handle(GoogleSignInCommand command, CancellationToken cancellationToken)
    {
        GoogleUserInfo userInfo = await googleAuthService.GetUserFromGoogleAsync(command.Code);

        IdentityUser? identityUser = await userManager.Users.Where(u => u.Email == userInfo.Email).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        UserDTO? userDTO = new()
        {
            FirstName = userInfo.Name,
            LastName = "",
            Bio = ""
        };

        if (identityUser is null)
        {
            identityUser = new()
            {
                Email = userInfo.Email,
                UserName = userInfo.Email,
                EmailConfirmed = userInfo.VerifiedEmail
            };

            var password = passwordGenerationService.GenerateRandomPassword();
            var result = await userManager.CreateAsync(identityUser, password);


            if (!result.Succeeded)
            {
                return Result<LoginDTO>.Error("Failed to register the user.");
            }
            var adddRoleresult = await userManager.AddToRoleAsync(identityUser, RolesNameValues.User);
            if (!adddRoleresult.Succeeded)
            {
                return Result<LoginDTO>.Error("Failed to assign role to the user.");
            }

            // @ToDo: add the new user data [NEED PLANNING]
        }
        else
        {
            var userDtoByIdentitySpec = new UserDtoByIdentitySpecification(identityUser.Id);

            userDTO = await unitOfWork.ApplicationUserRepository.FirstOrDefaultAsync(userDtoByIdentitySpec, cancellationToken);

            if (userDTO is null)
            {
                return Result<LoginDTO>.Error("Failed to retrieve the user data.");
            }
        }

        TokenRequest tokenRequest = new(
            UserId: userDTO.Id.ToString(),
            UserEmail: identityUser.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(identityUser)
        );

        string accessToken = tokenService.CreateAccessToken(tokenRequest);
        string refreshToken = await tokenService.CreateRefreshToken(identityUser.Id);

        return Result<LoginDTO>.Success(new LoginDTO(userDTO, accessToken, refreshToken));
    }
}
