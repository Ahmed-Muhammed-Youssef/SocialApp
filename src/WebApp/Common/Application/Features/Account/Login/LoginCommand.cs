using Mediator;
using Shared.Results;

namespace Application.Features.Account.Login;

public record LoginCommand(string Email, string Password) : ICommand<Result<LoginDTO>>;
