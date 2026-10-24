namespace Notifications.API.Endpoints;

public static class ListNotificationsEndpoint
{
    public static RouteHandlerBuilder MapListNotifications(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/notifications", HandleAsync)
            .RequireAuthorization()
            .WithName("List")
            .Produces<PagedResponse<NotificationResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<Ok<PagedResponse<NotificationResponse>>> HandleAsync(
        ClaimsPrincipal user,
        NotificationsDbContext db,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        string userId = user.GetUserId();

        IQueryable<UserNotification> query = db.UserNotifications
            .Where(un => un.RecipientId == userId)
            .OrderByDescending(un => un.CreatedAt)
            .AsNoTracking();

        int totalCount = await query.CountAsync(ct);

        List<NotificationResponse> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(un => new NotificationResponse
            {
                Id = un.NotificationId,
                Type = un.Notification!.Type,
                Title = un.Notification.Title,
                Body = un.Notification.Body,
                Data = un.Notification.Data,
                CreatedAt = un.CreatedAt,
                ReadAt = un.ReadAt,
            })
            .ToListAsync(ct);

        return TypedResults.Ok(new PagedResponse<NotificationResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }
}
