namespace Application.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<Result<RefreshTokenResult>>;
