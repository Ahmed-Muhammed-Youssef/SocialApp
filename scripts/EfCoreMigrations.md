# EF Core Migration Commands

This document contains the commands for creating and applying migrations using Entity Framework Core, with specific folder locations for each step.

## Initial Setup

Before you can start creating migrations, ensure you have the EF Core CLI tools installed. Run this command in any directory:

```
dotnet tool install --global dotnet-ef
```

## Creating Migrations

To create a new migration, navigate to the project directory containing your DbContext:

```
cd SoicalApp\src\WebApp\Common\Infrastructure
```

Then run:

```
dotnet ef migrations add MigrationName
```

Replace `MigrationName` with a descriptive name for your migration. For example:

```
dotnet ef migrations add InitialCreate -o Data/Migrations -c DataContext
```

## Applying Migrations

To apply the migrations to your database, ensure you're still in the project directory:

```
cd SoicalApp\src\WebApp\Common\Infrastructure
```

Then run:

```
dotnet ef database update -c DataContext
```

This command will apply any pending migrations to your database.
