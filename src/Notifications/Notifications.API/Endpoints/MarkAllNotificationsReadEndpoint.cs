namespace Notifications.API.Endpoints;

public static class MarkAllNotificationsReadEndpoint
{
    public static RouteHandlerBuilder MapMarkAllNotificationsRead(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/notifications/read-all", HandleAsync)
            .RequireAuthorization()
            .WithName("MarkAllRead")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<NoContent> HandleAsync(
        ClaimsPrincipal user,
        NotificationsDbContext db,
        CancellationToken ct)
    {
        string userId = user.GetUserId();
        DateTimeOffset now = DateTimeOffset.UtcNow;

        await db.UserNotifications
            .Where(un => un.RecipientId == userId && un.ReadAt == null)
            .ExecuteUpdateAsync(
                s => s.SetProperty(un => un.ReadAt, now), ct);

        return TypedResults.NoContent();
    }
}
