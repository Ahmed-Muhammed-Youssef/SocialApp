using Microsoft.AspNetCore.SignalR;
using Notifications.API.Hubs;
using Npgsql;

namespace Notifications.API.Endpoints;

public static class PushNotificationEndpoint
{
    public static RouteHandlerBuilder MapPushNotification(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/internal/notifications", HandleAsync)
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<PushNotificationRequest>>()
            .WithName("Push");
    }

    public static async Task<IResult> HandleAsync(
        PushNotificationRequest request,
        NotificationsDbContext db,
        IHubContext<NotificationsHub, INotificationsClient> hubContext,
        ILoggerFactory loggerFactory,
        CancellationToken ct)
    {
        var recipientIds = request.RecipientIds.Distinct().ToList();

        // Idempotency: a repeated (source, event_id) is ignored
        if (await db.Notifications.AnyAsync(n => n.Source == request.Source && n.EventId == request.EventId, ct))
        {
            return TypedResults.Ok();
        }

        DateTimeOffset now = DateTimeOffset.UtcNow;

        var notification = new Notification
        {
            Source = request.Source,
            EventId = request.EventId,
            Type = request.Type,
            Title = request.Title,
            Body = request.Body,
            Data = request.Data,
            CreatedAt = now,
            Recipients = recipientIds
                .Select(rid => new UserNotification
                {
                    RecipientId = rid,
                    CreatedAt = now,
                })
                .ToList(),
        };

        db.Notifications.Add(notification);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return TypedResults.Ok();
        }

        // Fan-out to connected clients in real-time
        var notificationDto = new NotificationResponse
        {
            Id = notification.Id,
            Type = notification.Type,
            Title = notification.Title,
            Body = notification.Body,
            Data = notification.Data,
            CreatedAt = notification.CreatedAt,
            ReadAt = null,
        };

        try
        {
            await hubContext.Clients.Users(recipientIds)
                .ReceiveNotification(notificationDto);
        }
        catch (Exception ex)
        {
            ILogger logger = loggerFactory.CreateLogger("Notifications.API.Endpoints.Push");
            logger.LogError(
                ex,
                "Notification {NotificationId} ({Source}/{EventId}) was saved but the live push failed",
                notification.Id,
                notification.Source,
                notification.EventId);
        }

        return TypedResults.Created();
    }
}
