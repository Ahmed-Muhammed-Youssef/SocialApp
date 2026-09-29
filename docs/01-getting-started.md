# 01 - Getting Started

This guide provides step-by-step instructions for setting up the SocialApp project locally for development.

## Prerequisites
Before you begin, ensure you have the following installed:
- **.NET 10 SDK**: The backend API is built on .NET 10.
- **Node.js (`^20.19`, `^22.12` or `>=24`) & npm**: Required to run the Angular 21 client.
- **SQL Server**: A local instance of SQL Server. (Alternatively, you can use the provided Docker Compose configuration to spin up a SQL Server container).
- **Docker & Docker Compose** (Optional, but recommended for running the database and integrations).

## External Dependencies
The application relies on a few external services. You will need:
1. **Cloudinary Account**: For profile picture storage. Get your `Cloud Name`, `API Key`, and `API Secret`.
2. **Google Cloud Console**: (Optional) For Google Sign-in. You'll need an OAuth 2.0 Client ID.

## 1. Clone the Repository
```bash
git clone https://github.com/Ahmed-Muhammed-Youssef/SocialApp.git
cd SocialApp
```

## 2. Setting Up the API (Backend)

We strongly recommend using the .NET Secret Manager for local development so that you don't accidentally commit sensitive credentials.

Navigate to the API project folder:
```bash
cd src/API/API
```

Initialize user secrets (if not already initialized):
```bash
dotnet user-secrets init
```

Add your secrets:
```bash
dotnet user-secrets set "ConnectionStrings:AppConnection" "Server=localhost,1433;Database=AppDb;User Id=sa;Password=Password123!;TrustServerCertificate=True"
dotnet user-secrets set "Cloudinary:CloudName" "your-cloud-name"
dotnet user-secrets set "Cloudinary:ApiKey" "your-api-key"
dotnet user-secrets set "Cloudinary:ApiSecret" "your-api-secret"
dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
```

The connection string above matches the SQL Server started by `docker compose up -d app-db`. Change it if you use your own instance.

Apply Database Migrations (EF Core):
In the `Development` environment the API applies pending migrations on startup, so this step is optional. To apply them manually, make sure SQL Server is running, then execute:
```bash
dotnet ef database update --project ../Infrastructure --startup-project .
```

Run the API:
```bash
dotnet run
```
The API will be available at `https://localhost:5001` (or another port specified in `launchSettings.json`).

## 3. Setting Up the Client (Angular)

Open a new terminal window and navigate to the client application:
```bash
cd src/UI
```

Install NPM packages:
```bash
npm install
```

Start the Angular development server:
```bash
npm start
```
The application will be available at `http://localhost:4200`.

## 4. Running via Docker Compose

If you prefer to run the entire stack (Database, API) via Docker, you can use the provided `docker-compose.yml` file from the root directory:

```bash
docker-compose up -d
```
> [!NOTE]
> Make sure to pass your Cloudinary and Google secrets as environment variables if you are running the API via Docker.

## 5. Seeding Test Data

A fresh database holds only reference data (roles, countries, cities). To get users, friendships and posts to work with, use the **SeedDB** console tool in `tools/SeedDB`. It can be run directly, or through the `scripts/seed-bench.ps1` wrapper, which provides fixed benchmark scales.

### Prerequisites
SeedDB refuses to run until the target database has its schema, roles and cities. Before the first seed, either:
- start the API once against that database; migrations and reference data are applied on startup, or
- apply the migrations yourself and run `scripts/seed_essential_data.sql`.

### Seeding at a benchmark scale (`scripts/seed-bench.ps1`)

From the repository root, in PowerShell:

```powershell
docker compose up -d app-db                      # SQL Server on localhost:1433
.\scripts\seed-bench.ps1 -Scale small            # seed on top of any existing seeded data
.\scripts\seed-bench.ps1 -Scale medium -Reset    # remove previously seeded data first
```

| Scale     |     Users | Friends/user | Posts/user | Posts | Purpose                                  |
| --------- | --------: | -----------: | ---------: | ----: | ---------------------------------------- |
| `small`   |       100 |           99 |         50 |   5 k | Fast iteration; matches today's baseline |
| `medium`  |     1 000 |          150 |        100 | 100 k | Realistic small app                      |
| `large`   |    10 000 |          300 |        100 |   1 M | Where the architecture should break      |
| `xlarge`  |   100 000 |          300 |        100 |  10 M | Future growth                            |
| `xxlarge` | 1 000 000 |          300 |        100 | 100 M | Future growth                            |

| Parameter     | Required | Default                                   | Description                                                 |
| ------------- | -------- | ----------------------------------------- | ----------------------------------------------------------- |
| `-Scale`      | Yes      | —                                         | One of the scales above                                     |
| `-Connection` | No       | The `docker compose` SQL Server (`localhost,1433`, database `AppDb`) | SQL Server connection string to seed |
| `-Reset`      | No       | Off                                       | Delete previously seeded data before seeding                |

Run `Get-Help .\scripts\seed-bench.ps1 -Examples` for usage examples. The script runs SeedDB in Release mode and returns SeedDB's exit code.

> [!WARNING]
> Seeding adds to existing seeded data, so seeding two scales one after another leaves a mix of both. Pass `-Reset` when you need a clean dataset of a single scale, e.g. before benchmarking. SeedDB writes through EF Core in batches of 500 rows, so `xlarge` and especially `xxlarge` take a long time and need tens of GB of disk in the Docker volume.

> [!TIP]
> If PowerShell reports that *running scripts is disabled on this system*, run `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` once, or start the script with `powershell -ExecutionPolicy Bypass -File .\scripts\seed-bench.ps1 -Scale small`.

### Running SeedDB directly

For custom sizes, or from a non-Windows shell, call the tool itself:

```bash
dotnet run --project tools/SeedDB -- --connection "<connection-string>" --users 100 --friends-per-user 10 --posts-per-user 10
dotnet run --project tools/SeedDB -- --connection "<connection-string>" --users 0 --reset   # only remove seeded data
dotnet run --project tools/SeedDB -- --help
```

| Option                   | Default  | Description                                                   |
| ------------------------ | -------- | ------------------------------------------------------------- |
| `--connection <value>`   | required | Connection string of the database to seed                     |
| `--users <n>`            | 100      | Number of users to create                                     |
| `--friends-per-user <n>` | 10       | Accepted friendships per user (capped at users − 1)           |
| `--posts-per-user <n>`   | 10       | Posts per user                                                |
| `--reset`                | off      | Delete previously seeded data before seeding                  |

Both `--flag value` and `--flag=value` are accepted.

### Logging in as a seeded user
Seeded accounts use the `@seed.local` email domain and share the password `Pwd12345`. `--reset` deletes only rows carrying that marker, so data you created by hand is never touched.

### Exit codes
| Code | Meaning |
| ---- | ------- |
| `0`  | Success |
| `2`  | Invalid arguments |
| `3`  | Prerequisites missing (schema, roles or cities not present) |
| `4`  | Cancelled (Ctrl+C). Data from phases that already finished stays in the database; run with `-Reset` / `--reset` to start clean |
