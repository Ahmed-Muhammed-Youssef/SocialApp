namespace API.Features.Auth.Requests;

/// <summary>
/// Data transfer object for user registration.
/// </summary>
public record RegisterRequest
{
    /// <summary>The user's first name.</summary>
    public required string FirstName { get; init; }
    /// <summary>The user's last name.</summary>
    public required string LastName { get; init; }
    /// <summary>The user's gender.</summary>
    public Gender Gender { get; init; }
    /// <summary>The user's email address.</summary>
    public required string Email { get; init; }
    /// <summary>The user's password.</summary>
    public required string Password { get; init; }
    /// <summary>The user's date of birth.</summary>
    public DateTime DateOfBirth { get; init; }
    /// <summary>The ID of the city the user resides in.</summary>
    public int CityId { get; init; }
}
