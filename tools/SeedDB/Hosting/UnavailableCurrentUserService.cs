using Application.Common.Interfaces;

namespace SeedDB.Hosting;

/// <summary>
/// Stands in for the HttpContext-backed <c>CurrentUserService</c>, which has no request to read
/// from in a console process. Seed stages address users by explicit id; reaching for an ambient
/// "current user" here means a stage is using an API code path that does not fit batch seeding,
/// so this fails loudly rather than returning a plausible-looking default such as user 0.
/// </summary>
internal sealed class UnavailableCurrentUserService : ICurrentUserService
{
    private const string Message =
        "There is no current user while seeding. Pass the user id explicitly instead of resolving ICurrentUserService.";

    public string GetEmail() => throw new InvalidOperationException(Message);

    public int GetPublicId() => throw new InvalidOperationException(Message);

    public IEnumerable<string> GetRoles() => throw new InvalidOperationException(Message);
}
