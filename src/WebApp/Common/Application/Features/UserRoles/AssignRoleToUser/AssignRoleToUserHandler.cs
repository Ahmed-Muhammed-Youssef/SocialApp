namespace Application.Features.UserRoles.AssignRoleToUser;

public class AssignRoleToUserHandler(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork) : ICommandHandler<AssignRoleToUserCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user == null)
        {
            return Result<object?>.NotFound("User not found"); 
        }

        IdentityUser? identityUser = await userManager.FindByIdAsync(user.IdentityId);

        if(identityUser == null)
        {
            return Result<object?>.NotFound("User not found in identity store");
        }
        
        IdentityRole? role = await roleManager.FindByIdAsync(user.IdentityId);

        if (role == null || role.Name is null)
        {
            return Result<object?>.NotFound("Role not found");
        }

        IdentityResult result = await userManager.AddToRoleAsync(identityUser, role.Name);

        if (!result.Succeeded)
        {
            return Result<object?>.Error(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }

        return Result<object?>.Success(null);
    }
}
