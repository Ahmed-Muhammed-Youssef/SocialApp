namespace API.Features.Auth.Requests;

/// <summary>
/// Request to sign in using a Google OAuth token.
/// </summary>
/// <param name="Credential">The Google ID token.</param>
public record GoogleSignInRequest(string Credential);
