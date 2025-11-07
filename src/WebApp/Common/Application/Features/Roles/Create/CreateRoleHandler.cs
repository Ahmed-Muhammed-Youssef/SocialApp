namespace Application.Features.Roles.Create;

public class CreateRoleHandler(RoleManager<IdentityRole> roleManager) : ICommandHandler<CreateRoleCommand, Result<string>>
{
    public async ValueTask<Result<string>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var newRole = new IdentityRole() { Name = command.Name };
        var result = await roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            return Result<string>.Error(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
        }
        else
        {
            return Result<string>.Created(newRole.Id);
        }
    }
}
