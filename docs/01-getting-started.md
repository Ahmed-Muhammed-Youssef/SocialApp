# 01 - Getting Started

This guide provides step-by-step instructions for setting up the SocialApp project locally for development.

## Prerequisites
Before you begin, ensure you have the following installed:
- **.NET 10 SDK**: The backend API is built on .NET 10.
- **Node.js (v18+) & npm**: Required to run the Angular client.
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
dotnet user-secrets set "ConnectionStrings:AppConnection" "Server=localhost;Database=SocialApp;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True"
dotnet user-secrets set "Cloudinary:CloudName" "your-cloud-name"
dotnet user-secrets set "Cloudinary:ApiKey" "your-api-key"
dotnet user-secrets set "Cloudinary:ApiSecret" "your-api-secret"
dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
```

Apply Database Migrations (EF Core):
Ensure your SQL Server is running, then execute:
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
cd src/SocialApp
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
