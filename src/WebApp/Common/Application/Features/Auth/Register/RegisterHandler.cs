using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Features.Users;
using Domain.Constants;
using Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Auth.Register;

public class RegisterHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ITokenProvider tokenService) : ICommandHandler<RegisterCommand, Result<RegisterDTO>>
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

        ApplicationUser newApplicationUser = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Sex = command.Sex,
            DateOfBirth = command.DateOfBirth,
            CityId = command.CityId,
            IdentityId = newIdentityUser.Id
        };

        try
        {
            await unitOfWork.ApplicationUserRepository.AddAsync(newApplicationUser, cancellationToken);
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

        string token = tokenService.Create(tokenRequest);

        RegisterDTO dto = new(userData, token);

        return Result<RegisterDTO>.Created(dto);
    }
}
