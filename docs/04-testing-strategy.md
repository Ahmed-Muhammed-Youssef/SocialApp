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
dotnet test SocialApp.slnx
```

*(Note: Running integration tests requires Docker Desktop or an equivalent container runtime to be running locally so Testcontainers can start the SQL Server container).*
