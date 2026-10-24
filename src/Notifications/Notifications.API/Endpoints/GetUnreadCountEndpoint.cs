namespace Notifications.API.Endpoints;

public static class GetUnreadCountEndpoint
{
    public static RouteHandlerBuilder MapGetUnreadCount(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/notifications/unread-count", HandleAsync)
            .RequireAuthorization()
            .WithName("UnreadCount")
            .Produces<UnreadCountResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<Ok<UnreadCountResponse>> HandleAsync(
        ClaimsPrincipal user,
        NotificationsDbContext db,
        CancellationToken ct)
    {
        string userId = user.GetUserId();

        int count = await db.UserNotifications
            .AsNoTracking()
            .CountAsync(un => un.RecipientId == userId && un.ReadAt == null, ct);

        return TypedResults.Ok(new UnreadCountResponse(count));
    }
}
