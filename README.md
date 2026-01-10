# Social App
This repository contains two main web applications built with **ASP.NET Core on .NET 10**:
1. **API Application** - Provides endpoints for client apps with JWT-based authentication.
2. **Angular SPA (Client)** - A single-page application built with Angular that consumes the API. The client uses JWT for authentication and real-time features via SignalR.

## Features

### Common Features
- **User Management**:
  - Registration and authentication.
  - JWT Tokens for API consumption.
- **Real-Time Messaging**:
  - Chat functionality powered by SignalR.
- **Friendship System**:
  - Friend requests and connections.
- **User Profiles**:
  - Editable profiles.
  - Profile picture uploads using **Cloudinary** as blob storage.

### API-Specific Features
- **Admin Role Management**:
  - Admin users can assign roles to other users.
- **Caching**:
  - Heavy endpoints implement caching for enhanced performance.

### Client (Angular) Specific Features
- **Single Page Application**:
  - Built with Angular and served separately from the API.
  - Uses JWT to authenticate against the API and connects to SignalR hubs for real-time updates.

## Prerequisites
- **.NET 10 SDK** installed.
- **Node.js** and **npm** (for the Angular client).
- **SQL Server** for database management.
- A **Cloudinary account** for storing profile pictures.
- A **Google account** to use the sign-in with Google service [optional].

## To Do / Production readiness checklist
This project is functional for development but requires the following changes before production deployment. Prioritize the top items first.

1) Docker & container hardening
- Use multi-stage Dockerfiles and smaller base images; run as non-root user.
- Fix port/service mismatches in `docker-compose.yml` and add health checks/resource limits.
- Ensure containers communicate over a dedicated network and service names are resolvable.

2) Secrets & configuration
- Remove secrets from source. Use environment variables or a secrets manager (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault).
- Use strongly typed settings (`IOptions<T>`) with validation.

3) Security hardening
- Enforce HTTPS, HSTS, secure headers and CSP in production.
- Restrict CORS to known origins and avoid `AllowAnyOrigin()` with `AllowCredentials()`.
- Protect Swagger UI in non-development environments (require auth or disable).
- Use stronger password policies or delegate auth to an identity provider.

4) Logging & correlation
- Add structured logging (Serilog) with enrichers for environment, request id and trace id.
- Send logs to a centralized sink (Seq/ELK/Azure Monitor) and correlate with traces.

5) Resiliency & reliability
- Add `Polly` policies for HTTP calls (retry/circuit-breaker/timeout).
- Configure EF Core retry and timeouts for transient failures.
- Persist ASP.NET Core data protection keys for multi-instance deployments.
- Replace in-memory cache with Redis for distributed caching.

6) Scalability & SignalR
- Use a SignalR backplane (Redis or Azure SignalR Service) for scale-out.
- Avoid per-instance session state; use distributed stores when required.

7) Database & migrations
- Use automated, controlled migrations during deploys; do not use `EnsureCreated` in production.
- Use least-privilege DB credentials and secure network access.

8) API stability & protection
- Add API versioning.
- Add rate limiting and request size limits.
- Map exceptions to `ProblemDetails` consistently and validate inputs thoroughly.

9) CI/CD and testing
- Add CI pipeline that builds, runs unit/integration tests, scans dependencies and publishes images.
- Add staging environment with smoke/e2e tests and migration orchestration.

10) Monitoring, SLOs and alerts
- Define SLOs (latency, error rates, availability) and configure alerts connected to telemetry backend.

11) Dependency management & code hygiene
- Pin package versions and keep dependencies up to date; run automated dependency scanning.
- Review DI lifetimes to prevent scoped into singleton issues and keep controllers thin.

Quick immediate implementation tasks (small PRs)
- Add Serilog basic configuration to `src/API/API/Program.cs`.

## Getting Started (development)
* Clone the repository
* Create a [Cloudinary](https://cloudinary.com/) account
* To enable Google sign-in, create a Google Cloud project and OAuth credentials.
* Add required secrets in user-secrets or environment variables (do NOT commit secrets).
* Add your SQL database connection string in the `appsettings` or appropriate configuration (infrastructure project).

API
* Run `dotnet run` in the `src/API/API` folder to start the API.

Angular client
* Change directory to the client app (e.g. `src/UI` or wherever the Angular project folder exists).
* Run `npm install` then `npm start` (or `ng serve`) to run the Angular development server.

Notes
* Ensure the client is configured to point to the API base URL and to use JWT for authentication.
* Update the client project path and start commands above if your Angular project is in a different location.
