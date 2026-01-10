namespace API.Features.Auth.Requests;

public record RegisterRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Gender Gender { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int CityId { get; init; }
}
