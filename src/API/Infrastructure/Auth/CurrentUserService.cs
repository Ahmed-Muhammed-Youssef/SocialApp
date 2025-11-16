namespace Infrastructure.Auth;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string GetEmail()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? throw new InvalidOperationException("Error retrieving Email from claims principal.");
    }

    public int GetPublicId()
    {
        Claim claim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Error retrieving Public ID from claims principal.");

        if (int.TryParse(claim.Value, out int id))
        {
            return id;
        }

        throw new InvalidOperationException("Error retrieving Public ID from claims principal.");
    }

    public IEnumerable<string> GetRoles()
    {
        return httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(r => r.Value) ?? [];
    }
}
