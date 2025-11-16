namespace Application.Features.Auth.Register;

public record RegisterCommand : ICommand<Result<RegisterDTO>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Gender Gender { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int CityId { get; init; }
}
