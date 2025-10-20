using Mediator;

namespace Application.Features.Account.Login;

public class LoginHandler : ICommandHandler<LoginCommand, LoginResult>
{
    public ValueTask<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
