using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Auth.External;

/// <summary>
/// Production implementation of Google credential validator.
/// Validates Google ID tokens against Google's public keys.
/// </summary>
public class GoogleCredentialValidator : IGoogleCredentialValidator
{
    private readonly string _googleClientId;

    public GoogleCredentialValidator(IConfiguration configuration)
    {
        _googleClientId = configuration["Authentication:Google:ClientId"]!;
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string credential)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(credential,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_googleClientId],
            });
        return payload;
    }
}
