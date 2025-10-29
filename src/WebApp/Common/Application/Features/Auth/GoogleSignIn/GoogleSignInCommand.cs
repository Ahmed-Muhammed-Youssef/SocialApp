using Application.Features.Auth.Login;
using Mediator;
using Shared.Results;

namespace Application.Features.Auth.GoogleSignIn;

public record GoogleSignInCommand(string Code) : ICommand<Result<LoginDTO>>;