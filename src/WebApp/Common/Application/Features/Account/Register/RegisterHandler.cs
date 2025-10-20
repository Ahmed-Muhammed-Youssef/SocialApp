using Mediator;
using Shared.Results;

namespace Application.Features.Account.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand, Result<RegisterDTO>>
{
    public ValueTask<Result<RegisterDTO>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
