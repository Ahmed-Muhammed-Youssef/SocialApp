namespace API.Features.Auth.Responses;

/// <summary>
/// Response containing a refreshed access token and a new refresh token.
/// </summary>
/// <param name="Token">The new JWT access token.</param>
/// <param name="RefreshToken">The new refresh token.</param>
public record RefreshTokenResponse(string Token, string RefreshToken);
