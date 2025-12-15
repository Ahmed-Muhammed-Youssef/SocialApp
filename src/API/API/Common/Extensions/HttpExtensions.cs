namespace API.Common.Extensions;

public static class HttpExtensions
{
    private const string RefreshTokenCookieName = "refreshToken";
    private static readonly PathString RefreshPath = "/api/auth/refresh";

    public static void AddPaginationHeader(this HttpResponse response, PaginationHeader paginationHeader, JsonSerializerOptions options)
    {
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }

    public static void AppendRefreshTokenCookie(this HttpResponse response, string refreshToken, DateTime refreshTokenExpiresAtUtc)
    {
        var expires = new DateTimeOffset(
            DateTime.SpecifyKind(refreshTokenExpiresAtUtc, DateTimeKind.Utc),
            TimeSpan.Zero
        );

        response.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = RefreshPath,
                Expires = expires
            });
    }
}
