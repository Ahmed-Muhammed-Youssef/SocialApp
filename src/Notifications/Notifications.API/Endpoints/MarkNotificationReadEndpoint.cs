namespace Notifications.API.Endpoints;

public static class MarkNotificationReadEndpoint
{
    public static RouteHandlerBuilder MapMarkNotificationRead(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPatch("/notifications/{id:guid}/read", HandleAsync)
            .RequireAuthorization()
            .WithName("MarkRead")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<Results<NoContent, NotFound>> HandleAsync(
        Guid id,
        ClaimsPrincipal user,
        NotificationsDbContext db,
        CancellationToken ct)
    {
        string userId = user.GetUserId();

        UserNotification? un = await db.UserNotifications
            .FirstOrDefaultAsync(
                un => un.NotificationId == id && un.RecipientId == userId, ct);

        if (un is null)
        {
            return TypedResults.NotFound();
        }

        un.ReadAt ??= DateTimeOffset.UtcNow; // idempotent — preserves original read timestamp
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}
