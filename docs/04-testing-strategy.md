# 04 - Testing Strategy

The SocialApp relies on a robust testing suite divided into Unit Tests and Integration Tests to ensure high software quality and prevent regressions.

## 1. Unit Testing
Unit tests focus on isolated business logic and CQRS Handlers.
- **Frameworks:** xUnit (v3) is used as the core test runner.
- **Mocking:** `NSubstitute` is used to mock the `IUnitOfWork` and repository interactions.
- **Location:** The `test/unit` directory contains projects like `Application.Test`, `Domain.Test`, and `Shared.Test`.
- **Guidelines:** 
  - Ensure all `IAsyncLifetime` setup uses proper cancellation tokens (e.g., `TestContext.Current.CancellationToken`) to comply with xUnit v3 best practices.
  - Test the "Happy Path" and "Error Paths" for all handlers.

## 2. Integration Testing
Integration tests ensure that the API, Database, and external dependencies work together correctly.
- **Frameworks:** `xUnit` and `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`).
- **Testcontainers:** Instead of an In-Memory Database, we use `Testcontainers` (specifically `Testcontainers.MsSql`) to spin up an ephemeral SQL Server instance using Docker for *true* integration testing.
- **WireMock.Net:** External APIs (like Google OAuth token validation) are mocked using `WireMock.Net` to prevent flaky tests relying on external networks.
- **Location:** `test/integration/API.Test`.

## How to Run the Tests

To execute the entire test suite locally:

```bash
dotnet test --solution SocialApp.slnx                                                     # everything
dotnet test --project test/unit/Domain.Test                                               # one project
dotnet test --project test/unit/Application.Test --filter-class "*CreateFriendRequestHandler*"  # one class
dotnet test --project test/unit/Shared.Test --filter-method "*Success*"                   # matching methods
```

Tests run on **Microsoft.Testing.Platform** (selected in `global.json`), not VSTest. As a result, `dotnet test` takes `--solution` / `--project` rather than a bare path, and the VSTest filter `--filter "FullyQualifiedName~..."` does not work. Use xUnit v3's `--filter-class`, `--filter-method`, `--filter-namespace`, `--filter-trait` or `--filter-query` instead.

*(Note: Running integration tests requires Docker Desktop or an equivalent container runtime to be running locally so Testcontainers can start the SQL Server container).*

## 3. Performance Benchmarks
`benchmarks/API` contains BenchmarkDotNet micro-benchmarks for the data-access paths. `dotnet test` does not run them, and their numbers depend on how much data is in the database. Seed at a known scale first, with `-Reset` so data from a previous run doesn't skew the numbers:

```powershell
docker compose up -d app-db
.\scripts\seed-bench.ps1 -Scale medium -Reset
dotnet run -c Release --project benchmarks/API/API -- --filter '*PostQueriesBenchmark*'
```

See [01 - Getting Started § Seeding Test Data](01-getting-started.md#5-seeding-test-data) for the available scales, and [`benchmarks/README.md`](../benchmarks/README.md) for configuration and what the results do and don't measure.
