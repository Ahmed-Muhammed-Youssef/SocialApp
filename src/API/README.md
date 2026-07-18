# SocialApp Backend (ASP.NET Core / .NET 10)

The backend of SocialApp is a production-grade REST + real-time API built with **ASP.NET Core on .NET 10**. It powers user management, authentication (JWT + Google Sign-In), a friendship system, posts/newsfeed, profile pictures (Cloudinary), and real-time chat & presence via SignalR.

> This README covers the backend only. For repository-wide info (Angular client, full docs set), see the [root README](../../README.md) and the [`docs/`](../../docs) directory.

---

## 📁 Solution Layout

The backend lives under `src/API` and follows **Clean Architecture** — dependencies point inward, and the core business logic knows nothing about frameworks, databases, or HTTP:

```text
src/API/
 ├── Domain/           # Aggregates, entities, domain rules & exceptions (no external dependencies)
 ├── Application/      # CQRS commands/queries + handlers, DTOs, repository & service interfaces
 ├── Infrastructure/   # EF Core DbContext, repositories, identity/token services, Cloudinary, stores
 ├── API/              # ASP.NET Core host: controllers, SignalR hubs, middleware, filters, DI wiring
 └── Shared/           # Cross-cutting building blocks: Result, Specification, Pagination, EntityBase
```

Related backend assets elsewhere in the repo:

| Path | Purpose |
|---|---|
| `test/unit/` | Unit tests per layer (`Domain.Test`, `Application.Test`, `Infrastructure.Test`, `Shared.Test`) |
| `test/integration/API.Test` | End-to-end API tests (`WebApplicationFactory` + Testcontainers SQL Server) |
| `benchmarks/API` | BenchmarkDotNet micro-benchmarks (e.g., `UsersControllerBenchmark`) |
| `scripts/` | `EfCoreMigrations.md` cheat-sheet and `seed_essential_data.sql` |
| `src/API/API/Dockerfile` | Container image for the API (used by `docker-compose.yml`) |

---

## 🏛 Architecture & Patterns

- **Clean Architecture** — `Domain` has zero framework dependencies; `Application` depends only on `Domain`/`Shared`; `Infrastructure` implements the interfaces `Application` defines; `API` composes everything.
- **Domain-Driven Design** — aggregate roots (`ApplicationUser`, `DirectChat`, `Friend`, `FriendRequest`, `Picture`, `RefreshToken`) with private setters, guarded constructors, and behavior methods (`AddPost`, `AssociateWithIdentity`, `MarkActive`, …). Invariant violations throw dedicated domain exceptions (e.g., `InvalidFriendRequestException`, `InvalidMessageException`). Base types (`EntityBase`, `IAggregateRoot`, `DomainEventBase`, `HasDomainEvents`) live in `Shared`.
- **CQRS with source-generated Mediator** — the [`Mediator`](https://github.com/martinothamar/Mediator) source-generator package (not MediatR) dispatches commands/queries to handlers. Each use case is a vertical slice under `Application/Features/<Feature>/<UseCase>` (e.g., `Users/Posts/CreatePost`, `Auth/RefreshToken`).
- **Result pattern** — handlers return `Result<T>` (`Shared/Results`) instead of throwing for expected failures; controllers translate results to HTTP responses / `ProblemDetails`.
- **Repository + Unit of Work** — EF Core is abstracted behind `IApplicationUserRepository`, `IDirectChatRepository`, etc., with `IUnitOfWork` committing transactions. A **cached decorator** (`CachedUserRepository`) wraps the user repository with `IMemoryCache`.
- **Specification pattern** — reusable query specifications (`Shared/Specification`, evaluated by `SpecificationEvaluator`) keep filtering/projection logic out of repositories (e.g., `UserWithPicturesSpecification`, `CityDtoSpecification`).
- **Validation** — FluentValidation validators on API request models, executed by a global `ValidationFilter` MVC filter; failures return standardized `ProblemDetails`.

## 🛠 Tech Stack

| Concern | Technology |
|---|---|
| Runtime / framework | .NET 10, ASP.NET Core |
| Data access | EF Core 10 (SQL Server in development, SQLite fallback otherwise), Dapper for stored-procedure queries |
| Identity & auth | ASP.NET Core Identity (`IdentityUser` + roles), JWT bearer tokens, refresh tokens, Google Sign-In (`Google.Apis.Auth`) |
| Real-time | SignalR (chat + online presence hubs) |
| Media storage | Cloudinary (`CloudinaryDotNet`) for profile/user pictures |
| Validation | FluentValidation |
| Observability | OpenTelemetry (traces, metrics, logs) → OTLP/Aspire Dashboard in dev, Azure Monitor in production |
| API docs | Swagger / Swashbuckle (with XML comments) at `/swagger` in development |
| Resilience & security | Built-in ASP.NET Core rate limiter, CORS policy, HSTS, global exception handler |

Package versions are managed centrally in [`Directory.Packages.props`](../../Directory.Packages.props); build settings (`net10.0`, nullable enabled, analyzers on `AnalysisMode=All`, **warnings as errors**) come from [`Directory.Build.props`](../../Directory.Build.props).

---

## 🌐 API Surface

All controllers are thin: they validate the request, dispatch a Mediator command/query, and map the `Result` to an HTTP response.

| Controller | Route | Responsibilities |
|---|---|---|
| `AuthController` | `api/auth` | `register`, `login`, `refresh` (refresh-token rotation), `google-signin` |
| `UsersController` | `api/users` | List/search users (paged), get/update profile, user posts, user pictures CRUD, set profile picture |
| `PostsController` | `api/posts` | Newsfeed (paged), get post by id, create post |
| `FriendRequestsController` | `api/friendrequests` | List, send, accept, delete friend requests |
| `ChatController` | `api/chat` | List direct chats, get chat by id |
| `RolesController` | `api/roles` | List/create/delete roles (admin) |
| `UserRolesController` | `api/users/{userId}/roles` | Get/assign/remove roles for a user (admin) |
| `ReferenceDataController` | `api/referencedata` | Reference data lookups (`cities`) |

Paged endpoints expose pagination metadata via a custom `Pagination` response header (`Common/Headers/PaginationHeader.cs`).

### SignalR Hubs

| Hub | Route | Purpose |
|---|---|---|
| `OnlineUsersHub` | `/hubs/presence` | Tracks online users (backed by the singleton `OnlineUsersStore`) |
| `ChatHub` | `/hubs/message` | Direct chat: joins a per-conversation group on connect, streams history (`ReceiveMessages`), broadcasts `NewMessage`, and notifies offline recipients via `IMessageNotifier` |

Hubs authenticate with the same JWT: the bearer token is read from the `access_token` query string for `/hubs/*` paths.

### Operational Endpoints

- `GET /health/live` — liveness (self check)
- `GET /health/ready` — readiness (database check, custom JSON formatter)
- `GET /swagger` — interactive API docs (Development only)

---

## 🔐 Security

- **Authentication:** JWT bearer (issuer/audience/signing-key validated). Login issues an access token plus a **refresh token** persisted per user (`RefreshToken` aggregate) and rotated on `POST api/auth/refresh`.
- **Google Sign-In:** `POST api/auth/google-signin` validates the Google credential (`GoogleCredentialValidator`) and provisions the user through `UserProvisioningService`, which orchestrates Identity + domain-user creation.
- **Authorization policies:** `RequireAdminRole` and `RequireModeratorOrAdmin` guard role-management endpoints.
- **Rate limiting:** global fixed-window limiter — 100 requests/minute partitioned per authenticated user (host header for anonymous callers); rejections return `429` with a `Retry-After` hint.
- **CORS:** single `AllowSpecificOrigin` policy driven by the `Cors:AllowedOrigins` config section, with credentials allowed (required for SignalR).
- **Error handling:** `GlobalExceptionHandler` (`IExceptionHandler`) + `ProblemDetails` everywhere, enriched with a `requestId` for correlation. HSTS is enabled outside development.

## 📊 Observability

Configured in `API/DependencyInjection.cs` (`AddObservability`):

- **Tracing:** ASP.NET Core, HttpClient, and EF Core instrumentation.
- **Metrics:** ASP.NET Core, HttpClient, and .NET runtime instrumentation.
- **Logs:** OpenTelemetry logging with scopes and formatted messages (EF Core SQL command logs are filtered out since tracing already captures them).
- **Exporters:** OTLP in development — the bundled **Aspire Dashboard** container (`http://localhost:18888`) visualizes everything; **Azure Monitor** in production.

## 🗄 Data & Persistence

- `ApplicationDatabaseContext` (Infrastructure/Data) hosts both ASP.NET Core Identity tables and the domain aggregates, with per-entity configurations under `Data/Configurations`.
- **Migrations** live in `Infrastructure/Data/Migrations` and are applied automatically at startup by `DatabaseInitializer` (which also seeds required data). Several read paths use **stored procedures** (created via migrations) queried with Dapper for performance.
- See [`scripts/EfCoreMigrations.md`](../../scripts/EfCoreMigrations.md) for the migration CLI commands and [`scripts/seed_essential_data.sql`](../../scripts/seed_essential_data.sql) for reference-data seeding.

---

## 🚀 Running the Backend

### Prerequisites

- **.NET 10 SDK**
- **SQL Server** — local instance, or use the provided container: `docker compose up app-db`
- **Cloudinary account** (profile-picture upload features)
- **Google Cloud OAuth client** (optional, for Google Sign-In)

### Configuration & Secrets

Configuration sections used by the API (`appsettings.json` holds only non-secret defaults):

| Section | Purpose |
|---|---|
| `ConnectionStrings:AppConnection` | Database connection string |
| `Jwt` (`Issuer`, `Audience`, `Key`) | Token issuing/validation |
| `Cloudinary` | Cloud name / API key / API secret |
| `Authentication:Google` | Google OAuth endpoints & client settings |
| `Cors:AllowedOrigins` | Allowed SPA origins |

Store secrets with the .NET Secret Manager — **never commit them**:

```bash
cd src/API/API
dotnet user-secrets set "ConnectionStrings:AppConnection" "<connection-string>"
dotnet user-secrets set "Jwt:Key" "<long-random-key>"
dotnet user-secrets set "Cloudinary:ApiSecret" "<secret>"
```

### Run locally

```bash
cd src/API/API
dotnet run
```

The API listens on `https://localhost:5001` / `http://localhost:5000` and opens Swagger in development. The database is migrated and seeded automatically on startup.

### Run with Docker Compose (full stack)

```bash
docker compose up --build
```

This starts:
- **api** — the API container (built from `src/API/API/Dockerfile`), exporting telemetry via OTLP
- **app-db** — SQL Server 2022 on `localhost:1433`
- **api.aspire-dashboard** — Aspire Dashboard for traces/metrics/logs at `http://localhost:18888`

---

## 🧪 Testing

```bash
dotnet test SocialApp.slnx        # run everything (root of the repo)
```

- **Unit tests** (`test/unit`) — xUnit v3 + NSubstitute; one project per layer. Handlers are tested for happy and error paths; domain aggregates for invariant enforcement.
- **Integration tests** (`test/integration/API.Test`) — xUnit v3 + `WebApplicationFactory`, with **Testcontainers** spinning up an ephemeral SQL Server and **WireMock.Net** stubbing external HTTP (e.g., Google auth). Covers the auth and user flows end-to-end. Docker must be running for these.
- **Benchmarks** (`benchmarks/API`) — BenchmarkDotNet console project targeting hot endpoints.

Full strategy: [`docs/04-testing-strategy.md`](../../docs/04-testing-strategy.md).

## 🔄 CI/CD

- [`dotnet-ci.yml`](../../.github/workflows/dotnet-ci.yml) — restore, build, and test the whole solution on pushes/PRs to `master`/`develop`.
- [`deploy-api.yml`](../../.github/workflows/deploy-api.yml) — publish and deploy the API to an **Azure Web App** on pushes to `master`.

## 📚 Further Reading

- [01 - Getting Started](../../docs/01-getting-started.md)
- [02 - Architecture](../../docs/02-architecture.md) (deep-dive into the layer design)
- [03 - API Guide](../../docs/03-api-guide.md) (auth flows, response conventions)
- [04 - Testing Strategy](../../docs/04-testing-strategy.md)
- [05 - Deployment Strategy](../../docs/05-deployment.md)
- [06 - Improvement Plan](../../docs/06-improvement-plan.md)
- [ARCHITECTURE_ENHANCEMENTS.md](../../ARCHITECTURE_ENHANCEMENTS.md) (refactoring roadmap)
