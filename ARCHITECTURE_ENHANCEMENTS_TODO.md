# Architecture — Missing Enhancements

- [x] Implement automatic validation pipeline using FluentValidation (global `ValidationFilter` added)
- [ ] Implement domain event dispatching in `UnitOfWork.CommitAsync()` (publish events via `IMediator`)
- [ ] Replace manual try-catch business error patterns with consistent `Result<T>` usage and specific exception handling
- [ ] Add structured logging (Serilog) and correlation IDs middleware for request tracing
- [ ] Improve caching: add key management, invalidation on updates, and consider Redis for production
- [ ] Add API versioning (`Microsoft.AspNetCore.Mvc.Versioning`) and update controllers/routes
- [ ] Expand test coverage: add integration tests (testserver) and increase unit tests for handlers
- [x] Add health checks (`AddHealthChecks()` + endpoints)
- [ ] Review DbContext separation (consolidate Identity + Application contexts or document rationale)
- [x] Add rate limiting middleware or policies (Microsoft rate limiting or AspNetCoreRateLimit)
- [x] Fix CORS to be configurable for non-Development environments (move `UseCors` out of dev-only block)
- [ ] Replace manual Identity cleanup with transactional flows (use `BeginTransaction/Commit/Rollback` in UoW)
- [ ] Implement domain-event-driven workflows for side effects (e.g., notifications, cache invalidation)
- [ ] Add API monitoring (Application Insights / OpenTelemetry) and centralized logging sinks
- [ ] Add migration & EF tooling guidance corrections in `scripts/EfCoreMigrations.md`

