using System.Security.Cryptography;

namespace Infrastructure.Auth;

public class TokenProvider(IOptions<JwtAuthOptions> options) : ITokenProvider
{
    private readonly JwtAuthOptions _jwtAuthOptions = options.Value;
    public string CreateAccessToken(TokenRequest tokenRequest)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, tokenRequest.UserEmail ?? ""),
            new(ClaimTypes.NameIdentifier, tokenRequest.UserId)
        };

        foreach (var role in tokenRequest.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtAuthOptions.Audience,
            Issuer = _jwtAuthOptions.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtAuthOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken CreateRefreshToken(string userId)
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
        string refreshToken = Convert.ToBase64String(randomBytes);

        return RefreshToken.Create(
            userId, 
            refreshToken, 
            DateTime.UtcNow.AddDays(_jwtAuthOptions.RefreshTokenExpirationDays));
    }
}
