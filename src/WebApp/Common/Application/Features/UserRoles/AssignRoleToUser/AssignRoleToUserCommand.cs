using Mediator;
using Shared.Results;

namespace Application.Features.UserRoles.AssignRoleToUser;

public record AssignRoleToUserCommand(int UserId, string RoleId) : ICommand<Result<object?>>;
