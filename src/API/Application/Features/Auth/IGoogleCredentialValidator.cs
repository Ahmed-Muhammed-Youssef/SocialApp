using Google.Apis.Auth;

namespace Application.Features.Auth;

/// <summary>
/// Abstraction for Google credential validation.
/// This allows tests to mock Google JWT validation without hitting Google's servers.
/// </summary>
public interface IGoogleCredentialValidator
{
    /// <summary>
    /// Validates a Google credential (ID token) and returns the payload.
    /// </summary>
    Task<GoogleJsonWebSignature.Payload> ValidateAsync(string credential);
}
