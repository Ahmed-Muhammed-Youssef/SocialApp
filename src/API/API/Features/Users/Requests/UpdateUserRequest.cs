namespace API.Features.Users.Requests;

/// <summary>
/// Request to update a user's profile.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>The updated first name.</summary>
    public required string FirstName { get; set; }
    /// <summary>The updated last name.</summary>
    public required string LastName { get; set; }
    /// <summary>The updated biography.</summary>
    public string? Bio { get; set; }
    /// <summary>The updated city ID.</summary>
    public required int CityId { get; set; }
}
