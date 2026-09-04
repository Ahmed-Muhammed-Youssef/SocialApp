using API.Benchmark.Helpers;
using Application.Features.Users.Posts;
using Application.Features.Users.Posts.GetById;
using Application.Features.Users.Posts.List;
using BenchmarkDotNet.Attributes;
using Domain.ApplicationUserAggregate;
using Shared.Pagination;
using Shared.Results;

namespace API.Benchmark.Benchmarks;

/// <summary>
/// Post read paths, measured at the handler boundary: handler → repository → EF Core → SQL Server.
/// </summary>
/// <remarks>
/// The HTTP pipeline (Kestrel, middleware, model binding, authorization, JSON serialisation) is excluded,
/// so these are not endpoint latencies. Mediator dispatch is excluded too — the source-generated mediator
/// costs a virtual call, which is noise beside a database round trip.
/// </remarks>
public class PostQueriesBenchmark : DatabaseBenchmarkBase
{
    /// <summary>
    /// Page 1 is the cheap case, so a deep page is measured as well: <c>GetNewsfeed</c> pages with
    /// <c>Skip</c>/<c>Take</c> over an ordered query, and the server still walks the skipped rows.
    /// Cost therefore grows with the page number instead of staying flat, which a page-1-only
    /// benchmark would never reveal.
    /// </summary>
    [Benchmark(Description = "GetNewsfeed")]
    [Arguments(1, 10)]
    [Arguments(1, 50)]
    [Arguments(100, 10)]
    public ValueTask<Result<PagedList<PostDTO>>> GetNewsfeed(int pageNumber, int itemsPerPage)
    {
        GetPostsHandler handler = new(UnitOfWork, CurrentUser);
        PaginationParams pagination = new() { PageNumber = pageNumber, ItemsPerPage = itemsPerPage };

        return handler.Handle(new GetPostsQuery(pagination), CancellationToken.None);
    }

    [Benchmark(Description = "GetPostById")]
    public ValueTask<Result<Post>> GetPostById()
    {
        GetPostByIdHandler handler = new(UnitOfWork);

        return handler.Handle(new GetPostByIdQuery(BenchmarkHost.PostId), CancellationToken.None);
    }
}
