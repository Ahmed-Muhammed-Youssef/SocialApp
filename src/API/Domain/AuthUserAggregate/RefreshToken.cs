namespace Domain.AuthUserAggregate;

public class RefreshToken : EntityBase<Guid>
{
    private RefreshToken() { }

    public string UserId { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public IdentityUser? User { get; private set; }

    public static RefreshToken Create(string userId, string token, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId is required", nameof(userId));
        if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token is required", nameof(token));

        return new RefreshToken
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };
    }
}
