using System.Security.Cryptography;

namespace Notifications.API.Features.Push;

/// <summary>Bound from <c>Intake</c>.</summary>
public sealed class IntakeOptions
{
    /// <summary>Shared secret that internal producers (the main API) send in <see cref="ApiKeyEndpointFilter.HeaderName"/>.</summary>
    public string ApiKey { get; set; } = string.Empty;
}

/// <summary>Restricts the push endpoint to internal producers; end users authenticate to the hub with their JWT instead.</summary>
public sealed class ApiKeyEndpointFilter(IOptions<IntakeOptions> options) : IEndpointFilter
{
    public const string HeaderName = "X-Api-Key";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string expected = options.Value.ApiKey;
        string? provided = context.HttpContext.Request.Headers[HeaderName];

        if (string.IsNullOrEmpty(expected) || string.IsNullOrEmpty(provided) || !FixedTimeEquals(expected, provided))
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }

    // Comparing hashes keeps the comparison constant-time regardless of the provided key's length.
    private static bool FixedTimeEquals(string expected, string provided)
        => CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(expected)), SHA256.HashData(Encoding.UTF8.GetBytes(provided)));
}
