namespace Infrastructure.Auth;

public class TokenService(IConfiguration _config, UserManager<IdentityUser> _userManager) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));

    public async Task<string> CreateTokenAsync(IdentityUser identityUser, int applicationUserId)
    {
        var roles = await _userManager.GetRolesAsync(identityUser);
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Email, identityUser.Email ?? ""),
            new(JwtRegisteredClaimNames.NameId, applicationUserId.ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
