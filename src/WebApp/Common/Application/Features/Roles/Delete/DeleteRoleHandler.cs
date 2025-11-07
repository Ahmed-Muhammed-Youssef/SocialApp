namespace Application.Features.Roles.Delete;

public class DeleteRoleHandler(RoleManager<IdentityRole> roleManager) : ICommandHandler<DeleteRoleCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var appRole = await roleManager.FindByIdAsync(command.Id);
        if (appRole == null)
        {
            return Result<object?>.NotFound("Role not found");
        }
        var result = await roleManager.DeleteAsync(appRole);
        if (!result.Succeeded)
        {
            return Result<object?>.Error(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }

        return Result<object?>.NoContent();
    }
}
