# Benchmarks

BenchmarkDotNet micro-benchmarks for the API's data-access paths (`benchmarks/API/API`, assembly
`API.Benchmark`). Part of `SocialApp.slnx`, so `dotnet build SocialApp.slnx` compiles it, but
`dotnet test` does not run it — benchmarks are executed on demand.

## Running

They need a reachable SQL Server with data in it. The simplest source is the local stack:

```bash
docker compose up -d            # SQL Server on localhost:1433
dotnet run -c Release --project benchmarks/API/API -- --filter '*PostQueriesBenchmark*'
```

`-c Release` is required — BenchmarkDotNet refuses to produce numbers from a Debug build.
Omitting `--filter` prompts for a selection; `--filter '*'` runs everything.

| Environment variable | Default | Purpose |
|---|---|---|
| `SOCIALAPP_BENCHMARK_CONNECTION` | the `docker compose` SQL Server | Connection string to benchmark against |
| `SOCIALAPP_BENCHMARK_USER_ID` | `1` | The seeded user the benchmarks run as |
| `SOCIALAPP_BENCHMARK_POST_ID` | `1` | The post fetched by single-post reads |

`GlobalSetup` fails fast with an explicit message if the database is unreachable or the user does not
exist, rather than letting every iteration throw and surfacing as an opaque BenchmarkDotNet failure.

Results are written to `BenchmarkDotNet.Artifacts/` as GitHub-flavoured Markdown and CSV. That folder is
gitignored, so to keep a baseline for diffing, copy the run you care about somewhere tracked rather than
force-adding it. Do not transcribe numbers into source comments — they cannot be compared and rot silently,
which is exactly what happened to the previous version of these benchmarks.

## What is measured

The handler boundary: **handler → repository → EF Core → SQL Server**. Benchmarks build the container
from the application's own `AddInfrastructureServices`, so repository and `DbContext` wiring changes flow
in automatically instead of leaving a hand-wired copy to go stale.

Each iteration takes a fresh DI scope (a new `DbContext`, empty change tracker) and clears the shared
`IMemoryCache`, so reads through `CachedUserRepository` report query cost rather than a cache hit that
only the first iteration ever paid for. `PostCommandsBenchmark` wraps each iteration in a transaction it
rolls back, so `CreatePost` does not grow the table mid-run or leave junk rows behind.

| Class | Covers |
|---|---|
| `PostQueriesBenchmark` | `GetNewsfeed` (page 1 and a deep page, two page sizes), `GetPostById` |
| `PostCommandsBenchmark` | `CreatePost`, committed then rolled back |
| `UserQueriesBenchmark` | `GetUsers` (paged + filtered), `GetUser` by id |

## What is not measured — read before quoting a number

These are **not endpoint latencies and not a throughput measurement.** They exclude Kestrel, the
middleware pipeline, model binding, authorization, JSON serialisation and the response write, so a 30%
improvement here can move user-visible p99 by nothing at all.

- **Single-threaded.** No connection-pool contention, lock convoys, thread-pool starvation or
  cross-request GC pressure. Code that looks fine at one operation at a time can still collapse under
  concurrency; a micro-benchmark cannot find that.
- **A dev machine is not Azure SQL.** Different CPU, storage, and — dominating everything — network. A
  local round trip versus cross-AZ TCP plus TLS can differ by an order of magnitude, and latency ratios
  between two implementations can invert between the two environments.
- **Warm steady state only.** Warmup is discarded by design, EF Core caches the compiled query and SQL
  Server caches the plan and pages. First-request and cold-start cost are invisible here.
- **Whatever data happens to be in the database.** Query cost is a function of row count and
  distribution. `GetNewsfeed` sorts and pages over a subquery — free at 50 rows, a different execution
  plan entirely at five million without a covering index on `Posts(UserId, DatePosted)`.
- **Tail latency is only partly visible.** P95 is reported, but BenchmarkDotNet removes outliers by
  design, and 20 iterations is a thin sample for a p99 claim.

For capacity and tail-latency questions, use a load test (k6, NBomber, Azure Load Testing) against a
deployed environment with production-shaped data, and the OpenTelemetry traces the API already emits.
Benchmarks here are a hypothesis generator; production telemetry is the evidence.
