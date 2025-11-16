namespace Application.Features.Auth.GoogleSignIn;

public record GoogleSignInCommand(string Code) : ICommand<Result<LoginDTO>>;