using Mediator;
using Shared.Results;

namespace Application.Features.Users.GetById;

public record GetUserByIdQuery(int Id) : IQuery<Result<UserDTO>>;
