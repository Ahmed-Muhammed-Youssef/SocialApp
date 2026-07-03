# Architecture & Enhancements

This document outlines the current architectural state of the SocialApp project and details pragmatic enhancements designed to showcase mid-level software engineering skills, focusing on maintainability, robust domain modeling, and system scalability.

## 1. Current Architecture Overview

- **Clean Architecture Principles:** The application enforces strict boundaries with separated `Domain`, `Application`, `Infrastructure`, and `API` layers, preventing infrastructure logic from polluting business rules.
- **Rich Domain Models (DDD):** Entities such as `ApplicationUser`, `FriendRequest`, and `Post` act as aggregate roots. They use private setters and expose explicit behaviors (methods/factories) to mutate state, ensuring domain invariants are always maintained.
- **CQRS Implementation:** The Application layer uses the Mediator pattern (via `Mediator.SourceGenerator`) to strictly separate read operations (Queries) from write operations (Commands).
- **Persistence:** Abstracts Entity Framework Core via a Unit of Work and Repository pattern, supporting explicit transaction management.

## 2. Mid-Level Engineering Enhancements

While the foundation is solid, the following enhancements elevate the codebase to demonstrate intermediate to advanced software engineering practices.

### A. Resolve Domain Logic Leakage (Domain Services)
- **Current State:** Some Application Command Handlers (like `CreateFriendRequestHandler`) still contain core domain rules (e.g., verifying if a user is already a friend, or if a pending request already exists).
- **Action:** Introduce **Domain Services** (e.g., `FriendshipService`) to orchestrate complex interactions involving multiple aggregates. Command handlers should be purely orchestrational (Load Aggregates -> Invoke Domain Service -> Commit UnitOfWork).

### B. Implement Domain Events (Event-Driven Workflows)
- **Current State:** The system handles side-effects synchronously and manually within command handlers, increasing tight coupling.
- **Action:** Introduce an in-process Domain Event dispatcher.
  - Aggregates should record events (e.g., `FriendRequestAcceptedEvent`).
  - The `UnitOfWork.CommitAsync()` method should gather and publish these events (via `IMediator` notifications) before or after committing to the database.
  - This pattern demonstrates an understanding of decoupled workflows and eventual consistency.

### C. Standardize Robust Error Handling (Result Pattern)
- **Current State:** Some parts of the system rely on throwing `DomainException` to handle business rule violations.
- **Action:** Broaden the use of the `Result<T>` pattern. Eliminate exceptions for expected control flow failures (like validation errors or business rule violations). This significantly improves performance and makes method signatures more honest.

### D. Performance & Scalability Considerations
- **Action (Caching):** Introduce caching strategies for high-read scenarios (like User Profiles or News Feeds) using `IDistributedCache` (backed by Redis or Memory cache).
- **Action (Pagination):** Implement cursor-based pagination (keyset pagination) for feed and search endpoints to handle large datasets efficiently, replacing offset-based pagination which degrades at scale.

### E. Observability & Diagnostics
- **Current State:** Basic OpenTelemetry packages are referenced.
- **Action:** Finalize structured logging (e.g., Serilog) and implement Correlation IDs middleware. This allows tracing a single request across the entire system, an essential skill for debugging production microservices or complex monoliths.

### F. Advanced Testing Strategy
- **Current State:** The project has an integration test structure (`API.Test`), but it can be expanded.
- **Action:** Increase integration test coverage using `Testcontainers` (already referenced in the solution) to spin up ephemeral SQL Server instances for testing. Ensure core CQRS flows are tested end-to-end via `WebApplicationFactory`.
