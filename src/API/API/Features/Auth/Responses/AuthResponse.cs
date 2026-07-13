namespace API.Features.Auth.Responses;

/// <summary>
/// Response containing user data and authentication token.
/// </summary>
public record AuthResponse
{
    /// <summary>The authenticated user's data.</summary>
    public required UserDTO UserData { get; init; }
    /// <summary>The JWT access token.</summary>
    public required string Token { get; init; }
}
