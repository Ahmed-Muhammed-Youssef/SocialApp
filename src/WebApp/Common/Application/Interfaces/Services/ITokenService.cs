using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(IdentityUser identityUser, int applicationUserId);
    }
}
