# Architecture & DDD Enhancements TODO

> [!WARNING]
> The current architecture exhibits a mix of **Rich Domain Models** and **Anemic Domain Models**, with significant **Business Logic Leakage** into the Application layer.

## 1. Domain Layer Violations (Critical)

- [ ] **Introduce Domain Services**
  - **Problem**: `CreateFriendRequestHandler` contains core domain logic (checking for existing requests, friendship status). This is "Application Logic leaking".
  - **Fix**: Create `FriendRequestService` in Domain to handle complex interactions between aggregates (e.g., `friendRequestService.SendRequest(sender, target)`).

- [ ] **Standardize Entity Implementation**
  - **Problem**: Inconsistency between `FriendRequest` (Good: Factory/Private Setters) and `Post` (Bad: Public Setters).
  - **Action**: Enforce a strict "No Public Setters" rule for all Domain Entities.

## 2. Application Layer Refactoring

- [ ] **Refactor Command Handlers (Remaining)**
  - **Problem**: Handlers should act as orchestrators rather than scripts.
  - **Fix**: Ensure handlers load aggregates, call domain methods, and save changes (already done for `CreatePostHandler`).

- [ ] **Implement Domain Events (Legacy Todo)**
  - **Status**: Previously identified but not implemented.
  - **Action**: Dispatch events (e.g., `FriendRequestAcceptedEvent`) from `UnitOfWork.CommitAsync()` or immediately after aggregate actions to trigger side effects (notifications) without coupling services.

## 3. Infrastructure & Cross-Cutting Concerns (Legacy + New)

- [ ] **Review Reference Handling**
  - **Problem**: `ApplicationUser` interacts with `CityId` and `Gender` (Enum). Ensure referenced data (Cities) are valid.
  - **Action**: Consider specialized Value Objects or validation services for reference checks.

- [ ] **Legacy Todo Items (Still Valid)**
  - [ ] Implement domain event dispatching in `UnitOfWork.CommitAsync()` (publish events via `IMediator`)
  - [ ] Replace manual try-catch business error patterns with consistent `Result<T>` usage and specific exception handling
  - [ ] Add structured logging (Serilog) and correlation IDs middleware for request tracing
  - [ ] Improve caching: add key management, invalidation on updates, and consider Redis for production
  - [ ] Add API versioning (`Microsoft.AspNetCore.Mvc.Versioning`) and update controllers/routes
  - [ ] Expand test coverage: add integration tests (testserver) and increase unit tests for handlers
  - [ ] Review DbContext separation (consolidate Identity + Application contexts or document rationale)
  - [ ] Replace manual Identity cleanup with transactional flows (use `BeginTransaction/Commit/Rollback` in UoW)

## 4. Completed Items (kept for history)
- [x] Implement automatic validation pipeline using FluentValidation (global `ValidationFilter` added)
- [x] Add health checks (`AddHealthChecks()` + endpoints)
- [x] Add rate limiting middleware or policies (Microsoft rate limiting or AspNetCoreRateLimit)
- [x] Implement domain-event-driven workflows for side effects (e.g., notifications, cache invalidation)
- [x] Add API monitoring (Application Insights / OpenTelemetry) and centralized logging sinks
- [x] Add migration & EF tooling guidance corrections in `scripts/EfCoreMigrations.md`
- [x] Fix CORS to be configurable for non-Development environments (move `UseCors` out of dev-only block)
- [x] Refactor `Post` to Rich Domain Model (Private setters, Factory methods, Validation)
- [x] Refactor `Picture`, `UserPicture`, `Message`, `RefreshToken`, and Reference Data (`Country`, `Region`, `City`) to Rich Domain Models
- [x] Align Post and Picture aggregate boundaries (Managed via `ApplicationUser`, logic moved to `ApplicationUserRepository`)
- [x] Extract Post constants (Max length) to `SystemPolicy`
