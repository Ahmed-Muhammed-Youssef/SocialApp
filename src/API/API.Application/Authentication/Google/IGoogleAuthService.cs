using API.Domain.Entities;

namespace API.Application.Authentication.Google
{
    public interface IGoogleAuthService
    {
        Task<AppUser> GetUserFromGoogleAsync(string code);
        string BuildGoogleSignInUrl();
    }
}
