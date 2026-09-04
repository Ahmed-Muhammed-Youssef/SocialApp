using Application.Common.Interfaces;

namespace API.Benchmark.Helpers;

/// <summary>
/// Stand-in for <c>Infrastructure.Auth.CurrentUserService</c>, which reads the caller's identity out of
/// <c>IHttpContextAccessor</c>. Benchmarks run outside a request, so there is no <c>HttpContext</c> to read from.
/// </summary>
/// <remarks>
/// Authentication and claim parsing are therefore excluded from every measurement. They are constant-cost
/// and not what these benchmarks investigate.
/// </remarks>
public sealed class BenchmarkCurrentUserService(int userId, string email, IReadOnlyCollection<string> roles) : ICurrentUserService
{
    public int GetPublicId() => userId;

    public string GetEmail() => email;

    public IEnumerable<string> GetRoles() => roles;
}
