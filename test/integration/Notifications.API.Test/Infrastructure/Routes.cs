namespace Notifications.API.Test.Infrastructure;

/// <summary>Centralised route constants so tests don't hard-code URL strings.</summary>
internal static class Routes
{
    private const string Base = "/notifications";
    private const string InternalBase = "/internal/notifications";

    /// <summary>POST — push a notification to one or more recipients (producer endpoint).</summary>
    public const string Push = InternalBase;

    /// <summary>GET — list the authenticated user's notifications (paginated).</summary>
    public const string List = Base;

    /// <summary>PATCH /notifications/{id}/read — mark one notification as read.</summary>
    public static string MarkRead(Guid id) => $"{Base}/{id}/read";

    /// <summary>POST /notifications/read-all — mark every unread notification as read.</summary>
    public const string MarkAllRead = $"{Base}/read-all";

    /// <summary>GET /notifications/unread-count — badge count.</summary>
    public const string UnreadCount = $"{Base}/unread-count";
}
