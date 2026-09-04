using API.Benchmark.Helpers;
using Application.Features.Users;
using Application.Features.Users.Get;
using Application.Features.Users.List;
using BenchmarkDotNet.Attributes;
using Domain.ApplicationUserAggregate;
using Shared.Pagination;
using Shared.Results;

namespace API.Benchmark.Benchmarks;

/// <summary>
/// User read paths — the port of the original <c>UsersControllerBenchmark</c>, moved off the controller
/// and onto the handlers so the measurement is not entangled with MVC plumbing.
/// </summary>
/// <remarks>
/// <c>GetUsers</c> goes straight to the repository; <c>GetUser</c> goes through <c>CachedUserRepository</c>,
/// whose cache the base class clears each iteration so this reports the query cost rather than a cache hit.
/// </remarks>
public class UserQueriesBenchmark : DatabaseBenchmarkBase
{
    [Benchmark(Description = "GetUsers (paged + filtered)")]
    [Arguments(10)]
    [Arguments(50)]
    public ValueTask<Result<PagedList<UserDTO>>> GetUsers(int itemsPerPage)
    {
        GetUsersQueryHandler handler = new(UnitOfWork, CurrentUser);
        UserParams userParams = new()
        {
            PageNumber = 1,
            ItemsPerPage = itemsPerPage,
            MinAge = 20,
            MaxAge = 25,
            OrderBy = OrderByOptions.LastActive,
        };

        return handler.Handle(new GetUsersQuery(userParams), CancellationToken.None);
    }

    [Benchmark(Description = "GetUser by id")]
    public ValueTask<Result<UserDTO>> GetUser()
    {
        GetUserHandler handler = new(UnitOfWork);

        return handler.Handle(new GetUserQuery(BenchmarkHost.UserId), CancellationToken.None);
    }
}
