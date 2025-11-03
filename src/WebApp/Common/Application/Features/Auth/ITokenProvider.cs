using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth;

public interface ITokenProvider
{
    string Create(TokenRequest tokenRequest);
}
