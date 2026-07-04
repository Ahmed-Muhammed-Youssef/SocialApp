# 06 - Improvement Plan

This document details pragmatic architectural enhancements to elevate the codebase towards mid-to-senior software engineering practices, specifically focusing on Domain-Driven Design (DDD), Clean Architecture, Observability, and Performance.

## 1. Domain Logic Leakage (Domain Services)

### The Problem
Currently, some Application Layer handlers enforce core domain rules. For instance, `CreateFriendRequestHandler` validates whether a friend request already exists or if users are already friends before proceeding. This logic spans multiple aggregates and shouldn't live in the Application Layer.

### The Solution: Domain Services
We need to introduce Domain Services to encapsulate business rules that span multiple aggregates or entities.

- **Current State (Handler)**
  ```csharp
  // Application/Features/FriendRequests/Create/CreateFriendRequestHandler.cs
  if (await unitOfWork.FriendRequestRepository.GetFriendRequestAsync(senderId, targetId) != null) {
      return Result<int>.Error("You already sent a friend request to this user.");
  }
  ```

- **Future State (Domain Service)**
  ```csharp
  // Domain/Services/FriendshipDomainService.cs
  public async Task<Result<FriendRequest>> CreateRequestAsync(int senderId, int targetId) {
      // Invariant checks inside the domain service
      if (await _friendRepository.AreFriendsAsync(senderId, targetId))
          return Result.Failure(DomainErrors.Friendship.AlreadyFriends);
          
      return FriendRequest.Create(senderId, targetId);
  }
  ```
The handler simply orchestrates by calling `FriendshipDomainService` and committing the `UnitOfWork`.

## 2. Implement Domain Events (Event-Driven Workflows)

### The Problem
Side effects are currently handled synchronously. In `ApplicationUser`, calling `MarkActive()` directly mutates state without notifying the broader system of meaningful business occurrences.

### The Solution
Introduce an in-process Domain Event dispatcher.

- **Enhance EntityBase**
  ```csharp
  public abstract class EntityBase {
      private readonly List<IDomainEvent> _domainEvents = new();
      public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

      protected void RaiseDomainEvent(IDomainEvent domainEvent) {
          _domainEvents.Add(domainEvent);
      }
      public void ClearDomainEvents() => _domainEvents.Clear();
  }
  ```
- **Example Usage**
  When a user creates a post, `RaiseDomainEvent(new PostCreatedEvent(Id))` will be called.
- **Dispatching**
  Update the `IUnitOfWork.CommitAsync()` method to gather all `DomainEvents` from tracked entities and dispatch them using MediatR's `IPublisher` before (or after) saving changes to the database.

## 3. Standardize Robust Error Handling (Result Pattern)

### The Problem
Domain layer logic (e.g., in `ApplicationUser` constructor) uses exceptions (`ArgumentException`, `InvalidOperationException`) for business rule violations. This is expensive and acts as hidden control flow.

### The Solution
Refactor entities to use factory methods that return `Result<T>` instead of throwing exceptions.

- **Example Refactoring**
  ```csharp
  public static Result<ApplicationUser> Create(string firstName, string lastName) {
      if (string.IsNullOrWhiteSpace(firstName)) {
          return Result.Failure<ApplicationUser>(DomainErrors.User.FirstNameRequired);
      }
      // Return new ApplicationUser
  }
  ```
Implement a `DomainErrors` static class to hold standardized error definitions and ensure API controllers return HTTP 400 Bad Request with `ProblemDetails` when a `Result.Failure` is encountered.

## 4. Performance & Scalability Considerations

### Caching
Introduce caching strategies for high-read scenarios:
- Caching User Profiles.
- Use `IDistributedCache` (backed by Redis or Memory cache) to avoid DB trips for data that rarely changes.

### Keyset (Cursor-Based) Pagination
Replace offset-based pagination (`Skip(x).Take(y)`) with cursor-based pagination for the News Feed and Search endpoints. Offset pagination causes performance degradation at scale when querying deep into large datasets.

## 5. Observability & Diagnostics

To support a production-ready application, we need to improve how we trace requests.
- **Structured Logging:** Implement Serilog to log structured JSON data instead of plain text, allowing for easier querying in log aggregators (e.g., Elasticsearch, Seq).
- **Correlation IDs:** Add a Correlation ID middleware to attach a unique identifier (like a GUID) to every incoming HTTP request. This ID must be attached to all logs created during the request's lifecycle to trace issues across layers or microservices.

## 6. Advanced Testing Strategy

### Integration Tests with Testcontainers
While Unit Tests verify isolated components, End-to-End CQRS flows need to be tested against a real database.
- Utilize `Testcontainers` (already referenced in the project) to dynamically spin up an ephemeral SQL Server container before running the test suite.
- Use `WebApplicationFactory` to test API Endpoints and ensure they correctly trigger MediatR Commands/Queries and interact with the database successfully.
