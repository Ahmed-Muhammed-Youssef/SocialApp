# Social App

This repository contains a modern, production-grade social web application built with **ASP.NET Core on .NET 10** and an **Angular** Single-Page Application (SPA) client.

## 🏛 Architecture & Engineering Practices

The backend is engineered to showcase mid-level to senior software engineering skills, focusing on maintainability and robustness:
- **Clean Architecture:** Strict separation of concerns across `Domain`, `Application`, `Infrastructure`, and `API` layers.
- **Domain-Driven Design (DDD):** Rich domain models with aggregate roots (e.g., `ApplicationUser`, `FriendRequest`, `Post`), private setters, and encapsulated business rules to maintain domain invariants.
- **CQRS Pattern:** The Application layer uses the Mediator pattern to strictly separate read operations (Queries) from write operations (Commands).
- **Persistence:** Abstracts Entity Framework Core via the Unit of Work and Repository patterns.

*(For detailed architectural plans and past/future refactoring, see [ARCHITECTURE_ENHANCEMENTS.md](./ARCHITECTURE_ENHANCEMENTS.md))*

## 📚 Full Documentation

For a comprehensive deep-dive into the project, please consult our full detailed documentation located in the `docs/` directory:

- [01 - Getting Started](file:///c:/Users/ahmed/source/repos/SocialApp/docs/01-getting-started.md)
- [02 - Architecture](file:///c:/Users/ahmed/source/repos/SocialApp/docs/02-architecture.md)
- [03 - API Guide](file:///c:/Users/ahmed/source/repos/SocialApp/docs/03-api-guide.md)
- [04 - Testing Strategy](file:///c:/Users/ahmed/source/repos/SocialApp/docs/04-testing-strategy.md)
- [05 - Deployment Strategy](file:///c:/Users/ahmed/source/repos/SocialApp/docs/05-deployment.md)

## ✨ Features

### Common Features
- **User Management**: Registration, authentication, and JWT integration.
- **Real-Time Messaging**: Chat functionality powered by SignalR.
- **Friendship System**: Friend requests and user connections.
- **User Profiles**: Editable profiles with picture uploads using **Cloudinary** as blob storage.

### API-Specific Features
- **Admin Role Management**: Admin users can assign roles to other users.
- **Caching**: Heavy endpoints implement caching for enhanced performance.
- **Google Sign-In**: Integration with Google OAuth for streamlined onboarding.

### Client (Angular) Features
- **Single Page Application**: Uses JWT to authenticate against the API and connects to SignalR hubs for real-time updates.

---

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

---

## 🛠 Prerequisites
- **.NET 10 SDK**
- **Node.js** and **npm** (for the Angular client)
- **SQL Server** (or Docker to run the provided SQL Server container via `docker-compose`)
- A **Cloudinary account** (for storing profile pictures)
- A **Google Cloud account** (optional, for Google Sign-In)

## 💻 Getting Started (Development)

1. **Clone the repository.**
2. **Configure Secrets**: 
   - Add your database connection string, Cloudinary credentials, and Google OAuth credentials using the .NET Secret Manager (`dotnet user-secrets`) or environment variables. **Do NOT commit secrets.**
3. **Run the API**:
   - Navigate to `src/API/API`.
   - Run `dotnet run`.
4. **Run the Angular Client**:
   - Navigate to `src/SocialApp` (or your Angular project folder).
   - Run `npm install`, then `npm start` (or `ng serve`).
