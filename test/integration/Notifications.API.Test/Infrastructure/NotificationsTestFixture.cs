using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Notifications.API.Data;
using Notifications.API.Entities;

namespace Notifications.API.Test.Infrastructure;

/// <summary>Base class for all Notifications integration tests. Provides authenticated HttpClient helpers.</summary>
public abstract class NotificationsTestFixture : IClassFixture<NotificationsWebAppFactory>
{
    protected readonly NotificationsWebAppFactory Factory;

    protected NotificationsTestFixture(NotificationsWebAppFactory factory)
    {
        Factory = factory;
    }

    /// <summary>Returns an unauthenticated client — useful for 401 tests.</summary>
    protected HttpClient CreateClient() => Factory.CreateClient();

    /// <summary>Returns the notifications stored for the given recipient, read straight from the database.</summary>
    protected async Task<List<UserNotification>> GetStoredNotificationsAsync(string recipientId)
    {
        using IServiceScope scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();

        return await db.UserNotifications
            .Include(un => un.Notification)
            .Where(un => un.RecipientId == recipientId)
            .ToListAsync(TestContext.Current.CancellationToken);
    }

    /// <summary>Returns a client with a valid JWT for the given user id.</summary>
    protected HttpClient CreateAuthenticatedClient(string userId = "user-1")
    {
        HttpClient client = CreateClient();
        string token = JwtHelper.Generate(userId);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}

/// <summary>Mints JWTs signed with the factory's test key so the Notifications API trusts them.</summary>
internal static class JwtHelper
{
    public static string Generate(string userId)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(NotificationsWebAppFactory.TestSigningKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.NameId, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        var token = new JwtSecurityToken(
            issuer: NotificationsWebAppFactory.TestIssuer,
            audience: NotificationsWebAppFactory.TestAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
