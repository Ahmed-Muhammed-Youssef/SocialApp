# EF Core Migration Commands

This document contains the commands for creating and applying migrations using Entity Framework Core.

## Initial Setup
Ensure you have the EF Core CLI tools installed:
```bash
dotnet tool install --global dotnet-ef
```

## Running Migrations
Run these commands from the `SocialApp` solution root folder (where `SocialApp.slnx` is).

### Creating a New Migration
Replace `MigrationName` with a descriptive name (e.g., `AddUserBio`).

```bash
dotnet ef migrations add MigrationName --project src/API/Infrastructure --startup-project src/API/API --output-dir Data/Migrations
```

### Applying Migrations to Database
This applies all pending migrations to the database configured in `appsettings.json`.

```bash
dotnet ef database update --project src/API/Infrastructure --startup-project src/API/API
```

### Removing Last Migration (If applied)
```bash
dotnet ef database update LastGoodMigrationName --project src/API/Infrastructure --startup-project src/API/API
dotnet ef migrations remove --project src/API/Infrastructure --startup-project src/API/API
```

### Generating SQL Script (For Production)
Use this to generate a script you can run on your Azure SQL Database if you don't want to run `dotnet ef database update` from the CI/CD pipeline.

```bash
dotnet ef migrations script --project src/API/Infrastructure --startup-project src/API/API --output scripts/deploy_db.sql --idempotent
```
