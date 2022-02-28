﻿using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}