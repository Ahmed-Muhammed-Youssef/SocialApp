namespace Domain.AuthUserAggregate;

public class RefreshToken : EntityBase<Guid>
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAtUtc { get; set; }

    public IdentityUser? User { get; set; }
}
