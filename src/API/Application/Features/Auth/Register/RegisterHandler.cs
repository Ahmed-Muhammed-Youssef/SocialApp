
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application.Features.Auth.Register;

public class RegisterHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ITokenProvider tokenService, IIdentityDbContext identityDbContext) : ICommandHandler<RegisterCommand, Result<RegisterDTO>>
{
    public async ValueTask<Result<RegisterDTO>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userManager.Users.AnyAsync(u => u.Email == command.Email, cancellationToken: cancellationToken))
        {
            return Result<RegisterDTO>.Error("The Email is already taken.");
        }

        IdentityUser newIdentityUser = new()
        {
            UserName = command.Email,
            Email = command.Email
        };

        // Create Identity User
        var result = await userManager.CreateAsync(newIdentityUser, command.Password);

        if (!result.Succeeded)
        {
            return Result<RegisterDTO>.Error("Failed to register the user.");
        }

        var adddRoleresult = await userManager.AddToRoleAsync(newIdentityUser, RolesNameValues.User);

        if (!adddRoleresult.Succeeded)
        {
            await userManager.DeleteAsync(newIdentityUser);

            return Result<RegisterDTO>.Error("Failed to register the user.");
        }

        // Create application user

        ApplicationUser newApplicationUser = new(newIdentityUser.Id, command.FirstName, command.LastName, command.DateOfBirth, command.Gender, command.CityId);

        try
        {
            unitOfWork.ApplicationUserRepository.Add(newApplicationUser);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await userManager.DeleteAsync(newIdentityUser);

            return Result<RegisterDTO>.Error("Failed to register the user.");
        }

        UserDTO userData = UserMappings.ToDto(newApplicationUser);

        TokenRequest tokenRequest = new(
            UserId: userData.Id.ToString(),
            UserEmail: newIdentityUser.Email ?? string.Empty,
            Roles: await userManager.GetRolesAsync(newIdentityUser)
        );

        string accessToken = tokenService.CreateAccessToken(tokenRequest);
        Domain.AuthUserAggregate.RefreshToken refreshToken = tokenService.CreateRefreshToken(newIdentityUser.Id);

        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        RegisterDTO dto = new(userData, accessToken, refreshToken.Token, refreshToken.ExpiresAtUtc);

        return Result<RegisterDTO>.Created(dto);
    }
}
