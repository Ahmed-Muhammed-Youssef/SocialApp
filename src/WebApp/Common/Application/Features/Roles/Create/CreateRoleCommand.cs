namespace Application.Features.Roles.Create;

public record CreateRoleCommand(string Name) : ICommand<Result<string>>;
