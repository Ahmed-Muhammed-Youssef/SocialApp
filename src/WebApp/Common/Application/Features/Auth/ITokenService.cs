using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth;

public interface ITokenService
{
    public Task<string> CreateTokenAsync(IdentityUser identityUser, int applicationUserId);
}
