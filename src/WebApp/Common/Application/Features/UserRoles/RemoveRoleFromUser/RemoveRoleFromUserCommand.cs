using Mediator;
using Shared.Results;

namespace Application.Features.UserRoles.RemoveRoleFromUser;

public record RemoveRoleFromUserCommand(int UserId, string RoleId) : ICommand<Result<object?>>;
