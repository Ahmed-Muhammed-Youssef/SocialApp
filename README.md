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

## Getting Started
* Clone the repository
* Create a [Cloudinary](https://cloudinary.com/) account
* To enable Google sign-in, you must have a Google account and a project in the Google Cloud Console platform.
* Add the following information either in the project secrets file (recommended) or in appsettings in both the API and MVC project in the following form:<br>
  ``` json
  "Cloudinary": {
    "CloudName": "Add you Cloudinary name", // Your unique Cloudinary account name.
    "APIKey": "Add your API key",  // API key from Cloudinary for authentication.
    "APISecret": "Add your API secret" // Secret key for Cloudinary API calls.
  },
  "JWT": {
    "Key": "add your random key here", // A random string used to sign and validate JWTs.
    "Issuer": "https://localhost:5001/api" // The base URL for the JWT token issuer (API project).
  },
  "Authentication": {
    "Google": {
      "ClientId": "your google client id", // Client ID from Google Cloud Console for OAuth.
      "ClientSecret": "your client secret", // Client secret from Google Cloud Console for OAuth.
      "RedirectUri": "https://localhost:5001/api/AccountExternal/callback-google" // Redirect URI for Google sign-in callback.
    }
  },
  "AdminCred": {
    "Email": "admin@test", // Admin email for creating the initial admin user.
    "Password": "Pwd12345" // Admin password for the initial admin user.
  }
  ```
* Add your SQL database connection string in the AppSettings.cs file in the infrastructure project
* Run `dotnet run` on the API folder to start the API or on the MVC folder to start the MVC project
