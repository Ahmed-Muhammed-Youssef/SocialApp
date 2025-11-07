namespace Application.Features.Users.Update;

public record UpdateUserCommand : ICommand<Result<UserDTO>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Bio { get; init; }
    public required int CityId { get; init; }
}
