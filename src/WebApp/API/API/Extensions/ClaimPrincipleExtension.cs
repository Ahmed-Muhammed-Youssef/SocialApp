using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimPrincipleExtension
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            return int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        }
        public static IEnumerable<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindAll(ClaimTypes.Role).Select(r => r.Value);
        }
    }
}
