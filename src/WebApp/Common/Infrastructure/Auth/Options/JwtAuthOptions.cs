namespace Infrastructure.Auth.Options;

public class JwtAuthOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public double ExpirationInMinutes { get; init; }
    public double RefreshTokenExpirationDays { get; init; }
}
