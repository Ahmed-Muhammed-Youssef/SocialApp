using Application.Features.Users.Posts.Create;
using BenchmarkDotNet.Attributes;
using Shared.Results;

namespace API.Benchmark.Benchmarks;

/// <summary>
/// Post write path: <c>CreatePostHandler</c> → load the author → <c>ApplicationUser.AddPost</c> → commit.
/// </summary>
/// <remarks>
/// The handler loads the author through <c>CachedUserRepository</c>. The base class empties the shared
/// memory cache each iteration, so this measures the real query rather than a cache hit that only the
/// first iteration ever paid for.
/// </remarks>
public class PostCommandsBenchmark : DatabaseBenchmarkBase
{
    private const string Content = "Benchmark post content.";

    /// <summary>
    /// <c>CreatePost</c> commits. Without a transaction each iteration would leave a row behind and the
    /// table would grow mid-run, so every measurement would be taken against a slightly different
    /// database — and the run would leave thousands of junk posts behind. Begin and rollback sit outside
    /// the measured region.
    /// </summary>
    protected override void OnIterationSetup()
        => UnitOfWork.BeginTransactionAsync().GetAwaiter().GetResult();

    protected override void OnIterationCleanup()
        => UnitOfWork.RollbackTransactionAsync().GetAwaiter().GetResult();

    [Benchmark(Description = "CreatePost (rolled back)")]
    public ValueTask<Result<ulong>> CreatePost()
    {
        CreatePostHandler handler = new(UnitOfWork, CurrentUser);

        return handler.Handle(new CreatePostCommand(Content), CancellationToken.None);
    }
}
