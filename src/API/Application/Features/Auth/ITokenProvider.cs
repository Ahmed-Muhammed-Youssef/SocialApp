namespace Application.Features.Auth;

public interface ITokenProvider
{
    string CreateAccessToken(TokenRequest tokenRequest);
    Task<string> CreateRefreshToken(string userId);
}
