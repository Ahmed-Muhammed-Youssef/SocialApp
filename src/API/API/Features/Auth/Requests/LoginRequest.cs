namespace API.Features.Auth.Requests;

/// <summary>
/// Data transfer object for user login.
/// </summary>
public record LoginRequest
{
    /// <summary>The user's email address.</summary>
    public required string Email { get; init; }
    /// <summary>The user's password.</summary>
    public required string Password { get; init; }
}
