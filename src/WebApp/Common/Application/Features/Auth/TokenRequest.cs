namespace Application.Features.Auth;

public record TokenRequest(string UserId, string UserEmail, IEnumerable<string> Roles);
