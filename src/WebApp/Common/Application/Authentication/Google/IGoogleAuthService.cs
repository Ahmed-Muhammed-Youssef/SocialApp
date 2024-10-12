using Application.Authentication.GoogleModels;

namespace Application.Authentication.Google
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo> GetUserFromGoogleAsync(string code);
        string BuildGoogleSignInUrl();
    }
}
