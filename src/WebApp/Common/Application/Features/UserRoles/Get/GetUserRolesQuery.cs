using Mediator;
using Shared.Results;

namespace Application.Features.UserRoles.Get;

public record GetUserRolesQuery(int UserId) : IQuery<Result<List<string>>>;
