namespace Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetPublicId(this ClaimsPrincipal principal)
        {
            try
            {
                var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return null;

                if (int.TryParse(claim.Value, out int id))
                {
                    return id;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
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
}
