namespace Application.Features.Auth;

public interface ITokenProvider
{
    string CreateAccessToken(TokenRequest tokenRequest);
    Domain.AuthUserAggregate.RefreshToken CreateRefreshToken(string userId);
}
