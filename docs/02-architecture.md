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
- **Mediator (source-generated):** CQRS commands and queries are dispatched by the [`Mediator`](https://github.com/martinothamar/Mediator) source generator (`Mediator.SourceGenerator`), **not MediatR**. Handlers implement `ICommandHandler`/`IQueryHandler` and return `ValueTask`.
- **Commands vs. Queries:**
  - *Commands* (Write operations) change system state and return `Result<T>`.
  - *Queries* (Read operations) return DTOs or ViewModels.
- **Validation:** `FluentValidation` validators run in a global MVC `ValidationFilter`, which validates request models before the controller action dispatches a command.

## 3. Infrastructure Layer
The `Infrastructure` layer handles external concerns.
- **Entity Framework Core:** We use EF Core for data access.
- **Unit of Work & Repositories:** Abstracted data access ensures the `Application` layer doesn't depend directly on EF Core `DbContext`. The `IUnitOfWork` ensures atomic transactions when modifying multiple aggregates.
- **External Services:** Implementations for Cloudinary (Image Upload) and Identity are housed here.

## 4. API Layer
The `API` layer acts as the entry point.
- **Controllers:** Thin controllers that map HTTP requests to Mediator queries/commands.
- **Middleware:** Global exception handling, Authentication (JWT), and CORS configuration.

## 5. Notifications Service
`src/Notifications/Notifications.API` is a separate, single-project service with its own **PostgreSQL** database (EF Core + Npgsql, snake_case naming). Producers call `POST /notifications/push`. Users receive notifications over SignalR at `/hubs/notifications`, authenticated with the same JWT as the API.

Its reliability comes from two tables:
- **`notifications`, the inbox.** `event_id` is unique, so a producer can retry safely: duplicates are ignored.
- **`deliveries`, an internal outbox.** It has one row per recipient per channel. The row is written in the same transaction as the notification.

A `DeliveryWorker` per channel claims due rows with `FOR UPDATE SKIP LOCKED` and a lease, sends them, and records the outcome. Outcomes are:
- retry with exponential backoff,
- park while the recipient is offline, replayed when they reconnect,
- wait for the client's acknowledgement, re-sending if it doesn't arrive,
- fail or expire.

Channels sit behind `INotificationChannel`, so FCM or email can be added alongside SignalR. See the [Notifications README](../src/Notifications/README.md) for the full state machine and contracts.

## Architectural Enhancements (Roadmap)
For details on upcoming refactoring and enhancements (like Domain Services, Domain Events, and enhanced Observability), please refer to the `ARCHITECTURE_ENHANCEMENTS.md` file in the root directory.
