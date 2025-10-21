namespace Application.Authentication.GoogleModels;

public class GoogleUserInfo
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool VerifiedEmail { get; set; }
    public string? PictureUrl { get; set; }
}
