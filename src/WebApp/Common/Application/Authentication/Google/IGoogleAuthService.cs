using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Google
{
    public interface IGoogleAuthService
    {
        Task<IdentityUser> GetUserFromGoogleAsync(string code);
        string BuildGoogleSignInUrl();
    }
}
