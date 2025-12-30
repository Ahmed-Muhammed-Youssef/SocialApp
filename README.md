# Social App
This repository contains two main web applications built with **ASP.NET Core on .NET 9**:
1. **API Application** - Provides endpoints for client apps with JWT-based authentication.
2. **MVC Application** - A user-facing web application with Identity-based authentication.

## Features

### Common Features
- **User Management**:
  - Registration and authentication.
  - JWT Tokens (API) and Identity-based login (MVC).
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

### MVC-Specific Features
- **Identity Authentication**:
  - Secure and user-friendly login and registration.

## Prerequisites
- **.NET 9 SDK** installed.
- **SQL Server** for database management.
- A **Cloudinary account** for storing profile pictures.
- A **Google account** to use the sign-in with Google service [optional]. 

## To Do / Production readiness checklist
This project is functional for development but requires the following changes before production deployment. Prioritize the top items first.

1) Observability
- Add NuGet: `OpenTelemetry.Exporter.OpenTelemetryProtocol`.
- Explicitly configure OTLP exporters for tracing and metrics in `DependencyInjection.AddObservability` (use `AddOtlpExporter` for tracing and metrics and read endpoint/protocol from config/env).
- Temporarily add `AddConsoleExporter()` to verify telemetry emission locally.
- Verify `docker-compose.yml` Aspire/OTLP endpoint, protocol and ports (service name and ports must match the exporter configuration).
- Consider adding an OpenTelemetry Collector in production for buffering and agent configuration.

2) Docker & container hardening
- Use multi-stage Dockerfiles and smaller base images; run as non-root user.
- Fix port/service mismatches in `docker-compose.yml` and add health checks/resource limits.
- Ensure containers communicate over a dedicated network and service names are resolvable.

3) Secrets & configuration
- Remove secrets from source. Use environment variables or a secrets manager (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault).
- Use strongly typed settings (`IOptions<T>`) with validation.

4) Security hardening
- Enforce HTTPS, HSTS, secure headers and CSP in production.
- Restrict CORS to known origins and avoid `AllowAnyOrigin()` with `AllowCredentials()`.
- Protect Swagger UI in non-development environments (require auth or disable).
- Use stronger password policies or delegate auth to an identity provider.

5) Logging & correlation
- Add structured logging (Serilog) with enrichers for environment, request id and trace id.
- Send logs to a centralized sink (Seq/ELK/Azure Monitor) and correlate with traces.

6) Resiliency & reliability
- Add `Polly` policies for HTTP calls (retry/circuit-breaker/timeout).
- Configure EF Core retry and timeouts for transient failures.
- Persist ASP.NET Core data protection keys for multi-instance deployments.
- Replace in-memory cache with Redis for distributed caching.

7) Scalability & SignalR
- Use a SignalR backplane (Redis or Azure SignalR Service) for scale-out.
- Avoid per-instance session state; use distributed stores when required.

8) Database & migrations
- Use automated, controlled migrations during deploys; do not use `EnsureCreated` in production.
- Use least-privilege DB credentials and secure network access.

9) API stability & protection
- Add API versioning.
- Add rate limiting and request size limits.
- Map exceptions to `ProblemDetails` consistently and validate inputs thoroughly.

10) CI/CD and testing
- Add CI pipeline that builds, runs unit/integration tests, scans dependencies and publishes images.
- Add staging environment with smoke/e2e tests and migration orchestration.

11) Monitoring, SLOs and alerts
- Define SLOs (latency, error rates, availability) and configure alerts connected to telemetry backend.

12) Dependency management & code hygiene
- Pin package versions and keep dependencies up to date; run automated dependency scanning.
- Review DI lifetimes to prevent scoped into singleton issues and keep controllers thin.

Quick immediate implementation tasks (small PRs)
- Fix telemetry exporter setup in `src/API/API/DependencyInjection.cs` and add exporter package to the `API` project.
- Correct `docker-compose.yml` Aspire endpoint and port mappings.
- Add Serilog basic configuration to `src/API/API/Program.cs`.
- Add a `health` readiness and liveness endpoints and ensure Kubernetes/compose health checks map to them.

## Getting Started (development)
* Clone the repository
* Create a [Cloudinary](https://cloudinary.com/) account
* To enable Google sign-in, create a Google Cloud project and OAuth credentials.
* Add required secrets in user-secrets or environment variables (do NOT commit secrets).
* Add your SQL database connection string in the `appsettings` or appropriate configuration (infrastructure project).
* Run `dotnet run` in the `src/API/API` folder to start the API or run the MVC project to start the web app.
