using Mediator;
using Shared.Results;

namespace Application.Features.Roles.List;

public record GetRolesQuery() : IQuery<Result<List<string>>>;
