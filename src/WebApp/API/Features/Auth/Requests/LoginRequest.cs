namespace API.Features.Auth.Requests;

public record LoginRequest
{
    [Required, EmailAddress]
    public required string Email { get; init; }
    [Required]
    public required string Password { get; init; }
}
