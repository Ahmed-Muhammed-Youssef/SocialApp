using Mediator;

namespace Application.Features.Account.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
