namespace Application.Features.Roles.Delete;

public record DeleteRoleCommand(string Id) : ICommand<Result<object?>>;
