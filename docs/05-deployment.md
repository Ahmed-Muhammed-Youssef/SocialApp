# 05 - Deployment Strategy

This document describes local deployment with Docker Compose and the GitHub Actions pipelines that deploy to Azure.

## 1. Local Deployment (Docker Compose)
For local testing and deployment, the project uses `docker compose`.

### Services Defined:
- **api** (`socialapp_api`): the ASP.NET Core API, built from `src/API/API/Dockerfile`. It connects to the database as `Server=app-db,1433;Database=AppDb` (set in `docker-compose.override.yml`).
- **app-db** (`socialapp_db`): SQL Server 2022, published on `localhost:1433` (user `sa`, password `Password123!`), with data kept in the `app-db-data` volume.
- **notifications-api** (`socialapp_notifications_api`): the notifications service, built from `src/Notifications/Notifications.API/Dockerfile` and published on `http://localhost:5010`. It connects to `notifications-db`. `Jwt:*` and `Intake:ApiKey` come from the mounted user-secrets store it shares with the API.
- **notifications-db** (`socialapp_notifications_db`): PostgreSQL 17, published on `localhost:5432` (database/user `notifications`, password `Password123!`), with data kept in the `notifications-db-data` volume.
- **api.aspire-dashboard** (`aspire_dashboard`): the Aspire Dashboard on `http://localhost:18888`, which receives OpenTelemetry traces, metrics and logs from the API and the notifications service.

### Running the Stack:
Ensure Docker is running on your machine, then execute:
```bash
docker compose up -d --build
```
To run only the database (e.g. for `dotnet run` or `scripts/seed-bench.ps1` on the host), use `docker compose up -d app-db`.

> [!IMPORTANT]
> When running via Docker, you must supply external API secrets (like Cloudinary and Google Client IDs) as environment variables inside the `docker-compose.override.yml` or a `.env` file. Do not commit sensitive tokens to version control.

## 2. CI/CD Pipeline (GitHub Actions)

| Workflow | Trigger | What it does |
| --- | --- | --- |
| `api-ci.yml` | Push to `develop`; PR to `master`/`develop`; called by `api-deploy.yml` | Restore, build and test `SocialApp.slnx` (unit + Testcontainers integration tests) |
| `api-deploy.yml` | Push to `master`, or manual | Run `api-ci.yml` → publish the API → apply EF Core migrations to the production database → deploy to the Azure Web App |
| `ui-ci.yml` | Push/PR to `master`/`develop` touching `src/UI/**` | Build the Angular client with the production configuration |
| `ui-deploy.yml` | Push/PR to `master` touching `src/UI/**`, or manual | Inject the production API URL into `environment.ts` and deploy to Azure Static Web Apps; manages PR preview environments |

Deploys happen only from `master`. Follow the `release/*` / `hotfix/*` flow in `CONTRIBUTING.md` rather than pushing there directly.

## 3. Database Migrations in Production
- In production, the `migrate-database` job in `api-deploy.yml` runs `dotnet ef database update` against the production database (`DB_CONNECTION_STRING` secret) *before* the new build is deployed.
- The API applies migrations on startup (`DatabaseInitializer`) **only in `Development`**. In every environment it seeds the essential reference data (roles, countries, cities); the admin and test users are seeded only in `Development`. This keeps several instances from racing to migrate at startup.
- Never call `context.Database.EnsureCreated()`: it bypasses migrations entirely.
- The notifications service follows the same rule: it migrates at startup only in `Development`. It isn't part of `api-deploy.yml` yet; production migrations for it have to run `dotnet ef database update --connection ...` (see `scripts/EfCoreMigrations.md`).
