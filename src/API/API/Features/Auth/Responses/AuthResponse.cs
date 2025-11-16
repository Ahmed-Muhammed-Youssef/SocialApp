namespace API.Features.Auth.Responses;

public record AuthResponse
{
    public required UserDTO UserData { get; init; }
    [Required]
    public required string Token { get; init; }
}
