using Mediator;
using Shared.Results;

namespace Application.Features.Users.Get;

public record GetUserQuery(int Id) : IQuery<Result<UserDTO>>;
