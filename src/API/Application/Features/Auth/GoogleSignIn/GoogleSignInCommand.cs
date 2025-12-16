namespace Application.Features.Auth.GoogleSignIn;

public record GoogleSignInCommand(string Credential) : ICommand<Result<LoginDTO>>;