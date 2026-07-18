# Social App

This repository contains a modern, production-grade social web application built with **ASP.NET Core on .NET 10** and an **Angular** Single-Page Application (SPA) client.

## 📂 Repository Structure

```text
├── src/API/        # .NET backend (Clean Architecture: Domain, Application, Infrastructure, API, Shared)
├── src/UI/         # Angular SPA client
├── test/           # Unit & integration test projects
├── benchmarks/     # BenchmarkDotNet performance benchmarks
├── docs/           # Full project documentation
└── scripts/        # EF Core migration notes & seed data
```

## 🏛 Backend

The backend showcases Clean Architecture, Domain-Driven Design, and CQRS, with JWT + Google authentication, SignalR real-time chat/presence, OpenTelemetry observability, and a Testcontainers-based integration test suite.

➡️ **See the dedicated [Backend README](./src/API/README.md)** for the full architecture description, API surface, configuration, and how to run it.

*(For architectural plans and past/future refactoring, see [ARCHITECTURE_ENHANCEMENTS.md](./ARCHITECTURE_ENHANCEMENTS.md))*

## 🖥 Frontend

The client is an **Angular 21** SPA using Angular Material, with JWT authentication (including Google Sign-In), a real-time chat powered by SignalR, newsfeed, and user profiles.

➡️ **See the dedicated [UI README](./src/UI/README.md)** for the project structure, routing, environment configuration, and how to run it.

## 📚 Full Documentation

For a comprehensive deep-dive into the project, consult the detailed documentation in the `docs/` directory:

- [01 - Getting Started](./docs/01-getting-started.md)
- [02 - Architecture](./docs/02-architecture.md)
- [03 - API Guide](./docs/03-api-guide.md)
- [04 - Testing Strategy](./docs/04-testing-strategy.md)
- [05 - Deployment Strategy](./docs/05-deployment.md)
- [06 - Improvement Plan](./docs/06-improvement-plan.md)

## 📚 Full Documentation

For a comprehensive deep-dive into the project, please consult our full detailed documentation located in the `docs/` directory:

- [01 - Getting Started](file:///c:/Users/ahmed/source/repos/SocialApp/docs/01-getting-started.md)
- [02 - Architecture](file:///c:/Users/ahmed/source/repos/SocialApp/docs/02-architecture.md)
- [03 - API Guide](file:///c:/Users/ahmed/source/repos/SocialApp/docs/03-api-guide.md)
- [04 - Testing Strategy](file:///c:/Users/ahmed/source/repos/SocialApp/docs/04-testing-strategy.md)
- [05 - Deployment Strategy](file:///c:/Users/ahmed/source/repos/SocialApp/docs/05-deployment.md)

## ✨ Features

- **User Management**: Registration, authentication (JWT), refresh tokens, and Google Sign-In.
- **Real-Time Messaging**: Direct chat and online presence powered by SignalR.
- **Friendship System**: Friend requests and user connections.
- **User Profiles**: Editable profiles with picture uploads using **Cloudinary** as blob storage.
- **Posts & Newsfeed**: Create posts and browse a paginated newsfeed.
- **Admin Role Management**: Admin users can create roles and assign them to other users.
- **Performance & Resilience**: Caching on heavy endpoints, rate limiting, and health checks.
- **Observability**: OpenTelemetry tracing/metrics/logging with Aspire Dashboard (dev) and Azure Monitor (prod).
- **Client (Angular)**: SPA that authenticates against the API with JWT and connects to SignalR hubs for real-time updates.

## 🚀 Upcoming Focus (Next Couple of Weeks)

To elevate this project to full production readiness and solidify it as a standout portfolio piece, we are actively focusing on the following core areas over the next couple of weeks:

1. **Solidifying Observability**
   - Finalizing structured logging (e.g., Serilog) and Correlation IDs middleware.
   - Implementing distributed tracing to ensure production-grade debugging and monitoring capabilities across the entire system.

2. **Increasing Unit & Integration Test Coverage**
   - Expanding the `API.Test` integration test suite using **Testcontainers** for ephemeral SQL Server instances.
   - Ensuring all core CQRS flows are tested end-to-end via `WebApplicationFactory` and xUnit v3 to guarantee stability.

3. **Creating a Deployment Pipeline (CI/CD)**
   - Building automated GitHub Actions workflows to compile code, run the extensive test suite, and scan dependencies.
   - Creating a continuous deployment pipeline to orchestrate Docker containers and database migrations safely.

## 🛠 Prerequisites

- **.NET 10 SDK**
- **Node.js** and **npm** (for the Angular client)
- **SQL Server** (or Docker to run the provided SQL Server container via `docker-compose`)
- A **Cloudinary account** (for storing profile pictures)
- A **Google Cloud account** (optional, for Google Sign-In)

## 💻 Getting Started (Development)

1. **Clone the repository.**
2. **Configure Secrets**:
   - Add your database connection string, Cloudinary credentials, and Google OAuth credentials using the .NET Secret Manager (`dotnet user-secrets`) or environment variables. **Do NOT commit secrets.** See the [Backend README](./src/API/README.md#configuration--secrets) for the exact configuration sections.
3. **Run the API**:
   - Navigate to `src/API/API`.
   - Run `dotnet run`.
4. **Run the Angular Client** (details in the [UI README](./src/UI/README.md)):
   - Navigate to `src/UI`.
   - Run `npm install`, then `npm start` (or `ng serve`) and open `http://localhost:4200`.
5. **Or run the full stack with Docker**:
   - `docker compose up --build` (API + SQL Server + Aspire Dashboard).
