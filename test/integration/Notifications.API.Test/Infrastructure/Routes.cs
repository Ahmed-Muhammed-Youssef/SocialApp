namespace Notifications.API.Test.Infrastructure;

/// <summary>Centralised route constants so tests don't hard-code URL strings.</summary>
internal static class Routes
{
    /// <summary>POST — push a notification to one or more recipients (producer endpoint).</summary>
    public const string Push = "/internal/notifications";
}
