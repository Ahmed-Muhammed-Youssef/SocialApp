# Architecture & Enhancements

This document outlines the current architectural state of the SocialApp project and details pragmatic enhancements designed to showcase mid-level software engineering skills, focusing on maintainability, robust domain modeling, and system scalability.

## 1. Current Architecture Overview

- **Clean Architecture Principles:** The application enforces strict boundaries with separated `Domain`, `Application`, `Infrastructure`, and `API` layers, preventing infrastructure logic from polluting business rules.
- **Rich Domain Models (DDD):** Entities such as `ApplicationUser`, `FriendRequest`, and `Post` act as aggregate roots. They use private setters and expose explicit behaviors (methods/factories) to mutate state, ensuring domain invariants are always maintained.
- **CQRS Implementation:** The Application layer uses the Mediator pattern (via `Mediator.SourceGenerator`) to strictly separate read operations (Queries) from write operations (Commands).
- **Persistence:** Abstracts Entity Framework Core via a Unit of Work and Repository pattern, supporting explicit transaction management.

## 2. Mid-Level Engineering Enhancements

While the foundation is solid, several pragmatic enhancements have been identified to elevate the codebase towards mid-to-senior software engineering practices.

> [!NOTE]
> For an in-depth breakdown of these improvements with specific code examples, please refer to the detailed implementation plan: [06 - Improvement Plan](file:///c:/Users/ahmed/source/repos/SocialApp/docs/06-improvement-plan.md).

### Summary of Enhancements:
1. **Resolve Domain Logic Leakage (Domain Services):** Refactoring complex logic out of CQRS handlers and into dedicated Domain Services.
2. **Implement Domain Events (Event-Driven Workflows):** Introducing an in-process Domain Event dispatcher in `EntityBase` and `UnitOfWork` to handle side effects in a decoupled manner.
3. **Standardize Robust Error Handling (Result Pattern):** Broadening the use of `Result<T>` and factory methods, eliminating exceptions for expected business rule violations.
4. **Performance & Scalability Considerations:** Introducing `IDistributedCache` for high-read scenarios and keyset (cursor-based) pagination for efficient data retrieval.
5. **Observability & Diagnostics:** Implementing Serilog for structured logging and Correlation ID middleware for comprehensive request tracing.
6. **Advanced Testing Strategy:** Utilizing `Testcontainers` alongside `WebApplicationFactory` for robust, ephemeral End-to-End CQRS integration testing.
