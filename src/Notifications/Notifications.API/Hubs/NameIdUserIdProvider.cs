using Microsoft.AspNetCore.SignalR;

namespace Notifications.API.Hubs;

/// <summary>
/// Resolves user ID for SignalR from either 'nameid' (standard in our JWTs) or ClaimTypes.NameIdentifier.
/// </summary>
public sealed class NameIdUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirstValue("nameid")
            ?? connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
