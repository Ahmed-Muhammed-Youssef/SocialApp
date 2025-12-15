using Application.Features.Auth;
using Infrastructure.Auth;
using Infrastructure.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Test.Auth;

public class TokenProviderTests
{
    [Fact]
    public void Create_ValidInput_ReturnsToken()
    {
        // Arrange
        TokenRequest tokenRequest = new("user123", "test@example.com", [RolesNameValues.Admin, RolesNameValues.User]);
        IOptions<JwtAuthOptions> options = Options.Create(new JwtAuthOptions
        {
            Key = new string('-', 64),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationInMinutes = 60,
            RefreshTokenExpirationDays = 7
        });

        // Act
        string token = new TokenProvider(options).CreateAccessToken(tokenRequest);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        // Validate token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = options.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Value.Audience,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        // Decode for additional direct inspection
        var jwt = (JwtSecurityToken)validatedToken;
        Assert.Equal(options.Value.Issuer, jwt.Issuer);
        Assert.Contains(options.Value.Audience, jwt.Audiences);

        // Extract claims
        var claims = principal.Claims.ToList();

        Assert.Contains(claims, c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
        Assert.Contains(claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == "user123");
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == RolesNameValues.Admin);
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == RolesNameValues.User);
    }

    [Fact]
    public void CreateRefreshToken_ValidInput_ReturnsRefreshToken()
    {
        // Arrange
        IOptions<JwtAuthOptions> options = Options.Create(new JwtAuthOptions
        {
            Key = new string('-', 64),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationInMinutes = 60,
            RefreshTokenExpirationDays = 7
        });
        string userId = "user123";
        // Act
        var refreshToken = new TokenProvider(options).CreateRefreshToken(userId);
        // Assert
        Assert.NotNull(refreshToken);
        Assert.Equal(userId, refreshToken.UserId);
        Assert.False(string.IsNullOrWhiteSpace(refreshToken.Token));
        Assert.True(refreshToken.ExpiresAtUtc > DateTime.UtcNow.AddDays(6)); // Should be valid for at least 6 days
    }
}
