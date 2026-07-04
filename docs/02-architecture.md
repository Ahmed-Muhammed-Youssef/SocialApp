# 02 - Architecture

The SocialApp project follows **Clean Architecture** principles to ensure that the core business logic is independent of frameworks, databases, and UI. The backend is structured into four primary layers, supported by Domain-Driven Design (DDD) and the CQRS pattern.

## High-Level Project Structure

```text
src/API/
 ├── Domain/           # Core Entities, Interfaces, Exceptions
 ├── Application/      # CQRS Handlers, Behaviors, DTOs
 ├── Infrastructure/   # EF Core DbContext, Repositories, External APIs
 ├── API/              # Controllers, Middleware, Dependency Injection
 └── Shared/           # Common utilities, Result<T> pattern
```

## 1. Domain Layer
At the heart of the application is the `Domain` layer.
- **Aggregate Roots:** Entities like `ApplicationUser`, `FriendRequest`, and `Post` act as aggregate roots.
- **Encapsulation:** Entities use `private` or `protected` setters. State changes are made through explicit methods (e.g., `user.AddPost("content")` rather than `user.Posts.Add(new Post())`).
- **Domain Exceptions:** Business rule violations throw specific domain exceptions (e.g., `InvalidFriendRequestException`).

## 2. Application Layer (CQRS)
The `Application` layer orchestrates business use cases.
- **MediatR:** We use `Mediator.SourceGenerator` for high-performance Command Query Responsibility Segregation (CQRS).
- **Commands vs. Queries:**
  - *Commands* (Write operations) change system state and return `Result<T>`.
  - *Queries* (Read operations) return DTOs or ViewModels.
- **Validation:** `FluentValidation` is integrated into the MediatR pipeline to automatically validate commands before they reach the handler.

## 3. Infrastructure Layer
The `Infrastructure` layer handles external concerns.
- **Entity Framework Core:** We use EF Core for data access.
- **Unit of Work & Repositories:** Abstracted data access ensures the `Application` layer doesn't depend directly on EF Core `DbContext`. The `IUnitOfWork` ensures atomic transactions when modifying multiple aggregates.
- **External Services:** Implementations for Cloudinary (Image Upload) and Identity are housed here.

## 4. API Layer
The `API` layer acts as the entry point.
- **Controllers:** Thin controllers that map HTTP requests to MediatR Queries/Commands.
- **Middleware:** Global exception handling, Authentication (JWT), and CORS configuration.

## Architectural Enhancements (Roadmap)
For details on upcoming refactoring and enhancements (like Domain Services, Domain Events, and enhanced Observability), please refer to the `ARCHITECTURE_ENHANCEMENTS.md` file in the root directory.
