using Application.Common.Interfaces;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Shared.Results;

namespace Application.Features.UserRoles.RemoveRoleFromUser;

public class RemoveRoleFromUserHandler(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork) : ICommandHandler<RemoveRoleFromUserCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(RemoveRoleFromUserCommand command, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            return Result<object?>.NotFound("User not found");
        }

        var identityUser = await userManager.FindByIdAsync(user.IdentityId);

        if (identityUser is null)
        {
            return Result<object?>.NotFound("User not found in identity store");
        }

        var role = await roleManager.FindByIdAsync(command.RoleId);

        if (role is null || role.Name is null)
        {
            return Result<object?>.NotFound("Role not found");
        }

        var result = await userManager.RemoveFromRoleAsync(identityUser, role.Name);

        if (!result.Succeeded)
        {
            return Result<object?>.Error(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }

        return Result<object?>.NoContent();
    }
}
