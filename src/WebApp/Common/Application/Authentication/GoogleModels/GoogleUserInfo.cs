namespace Application.Authentication.GoogleModels;

public record GoogleUserInfo
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public bool VerifiedEmail { get; init; }
    public string? PictureUrl { get; init; }
}
