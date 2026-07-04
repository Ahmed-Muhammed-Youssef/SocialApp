# 05 - Deployment Strategy

This document outlines the current local deployment mechanisms and the upcoming plans for a CI/CD pipeline.

## 1. Local Deployment (Docker Compose)
For local testing and deployment, the project uses `docker-compose`. 

### Services Defined:
- **api:** The main ASP.NET Core API application. Built via the Dockerfile in `src/API/API/Dockerfile`.
- **sqlserver:** The Microsoft SQL Server database.

### Running the Stack:
Ensure Docker is running on your machine, then execute:
```bash
docker-compose up -d --build
```
> [!IMPORTANT]
> When running via Docker, you must supply external API secrets (like Cloudinary and Google Client IDs) as environment variables inside the `docker-compose.override.yml` or a `.env` file. Do not commit sensitive tokens to version control.

## 2. Upcoming CI/CD Pipeline (GitHub Actions)
As part of our next major push, we will implement a fully automated CI/CD pipeline.

### Proposed Workflow:
1. **Continuous Integration (CI):**
   - Triggered on every pull request to the `develop` or `main` branches.
   - Steps: Checkout code -> Setup .NET 10 -> Restore Dependencies -> Run Unit and Integration Tests (using Testcontainers).
2. **Continuous Deployment (CD):**
   - Triggered on merges to the `main` branch.
   - Steps: Build Docker images -> Push to Container Registry (e.g., GitHub CR or Docker Hub) -> Execute EF Core migrations -> Deploy to the hosting environment.

## 3. Database Migrations in Production
Unlike development environments where you might run `dotnet ef database update` locally, production migrations must be handled carefully. 
- The CD pipeline should be responsible for orchestrating the migration script against the production database *before* the new API containers are rolled out, or via an init container.
- We strictly avoid calling `context.Database.EnsureCreated()` or applying migrations dynamically inside `Program.cs` for production workloads to prevent concurrent migration execution issues.
