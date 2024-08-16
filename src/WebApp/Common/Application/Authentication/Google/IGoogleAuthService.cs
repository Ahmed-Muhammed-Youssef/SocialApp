using Domain.Entities;

namespace Application.Authentication.Google
{
    public interface IGoogleAuthService
    {
        Task<AppUser> GetUserFromGoogleAsync(string code);
        string BuildGoogleSignInUrl();
    }
}
