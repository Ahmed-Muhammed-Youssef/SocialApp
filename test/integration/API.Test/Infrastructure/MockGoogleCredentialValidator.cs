using Google.Apis.Auth;
using Application.Features.Auth;

namespace API.Test.Infrastructure;

/// <summary>
/// Mock implementation of IGoogleCredentialValidator for testing.
/// This allows tests to control what Google payload is returned without hitting Google's servers.
/// </summary>
public class MockGoogleCredentialValidator : IGoogleCredentialValidator
{
    private readonly Dictionary<string, GoogleJsonWebSignature.Payload> _validCredentials;
    private readonly bool _throwException;

    public MockGoogleCredentialValidator(bool throwException = false)
    {
        _validCredentials = new();
        _throwException = throwException;
    }

    /// <summary>
    /// Registers a credential that will return a specific payload when validated.
    /// </summary>
    public void RegisterCredential(string credential, GoogleJsonWebSignature.Payload payload)
    {
        _validCredentials[credential] = payload;
    }

    /// <summary>
    /// Creates a credential and registers it with a payload in one call.
    /// </summary>
    public string CreateAndRegisterCredential(string email, string subject = "google-user-123", string givenName = "Google", string familyName = "User")
    {
        var credential = $"test-credential-{Guid.NewGuid()}";
        var payload = new GoogleJsonWebSignature.Payload
        {
            Email = email,
            GivenName = givenName,
            FamilyName = familyName,
            Subject = subject,
            EmailVerified = true,
            ExpirationTimeSeconds = (long)(DateTime.UtcNow.AddHours(1) - DateTime.UnixEpoch).TotalSeconds,
            IssuedAtTimeSeconds = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds,
        };

        RegisterCredential(credential, payload);
        return credential;
    }

    public Task<GoogleJsonWebSignature.Payload> ValidateAsync(string credential)
    {
        if (_throwException)
        {
            throw new InvalidOperationException("Mock: Invalid Google credential");
        }

        if (_validCredentials.TryGetValue(credential, out var payload))
        {
            return Task.FromResult(payload);
        }

        throw new InvalidOperationException($"Mock: Credential not registered: {credential}");
    }
}
