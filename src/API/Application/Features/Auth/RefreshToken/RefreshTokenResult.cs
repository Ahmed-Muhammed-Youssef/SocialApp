namespace Application.Features.Auth.RefreshToken;

public record RefreshTokenResult(string AccessToken, string NewRefreshToken, DateTime RefreshTokenExpiresAtUtc);
