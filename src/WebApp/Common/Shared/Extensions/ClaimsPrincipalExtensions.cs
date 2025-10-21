namespace Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetPublicId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Error retrieving Public ID from claims principal.");

        if (int.TryParse(claim.Value, out int id))
        {
            return id;
        }

        throw new Exception("Error retrieving Public ID from claims principal.");
    }

    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "";
    }
    public static IEnumerable<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindAll(ClaimTypes.Role).Select(r => r.Value);
    }
}
