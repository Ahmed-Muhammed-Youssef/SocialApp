namespace Application.Features.Auth;

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> GetUserFromGoogleAsync(string code);
}
