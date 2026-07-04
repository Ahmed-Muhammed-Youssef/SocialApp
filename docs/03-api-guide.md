# 03 - API Guide

This document covers how to interact with the SocialApp REST API, handling authentication, and understanding API responses.

## Authentication

The API uses **JSON Web Tokens (JWT)** for authentication. Some endpoints (like registration and login) are public, while most require a valid JWT in the `Authorization` header.

### 1. Standard Email/Password Flow
1. **Register:** `POST /api/auth/register`
2. **Login:** `POST /api/auth/login`
   - Returns an `access_token` and sets a `refreshToken` in a secure HTTP-only cookie.
3. **Use Token:** Pass the `access_token` in the header for subsequent requests:
   ```http
   Authorization: Bearer <your_jwt_token>
   ```

### 2. Google OAuth Flow
The application supports Google Sign-In.
1. The client (Angular) obtains a Google credential token.
2. The client sends the token to `POST /api/auth/google-signin`.
3. The API validates the Google token. If the user doesn't exist, an account is created. The API returns a JWT and sets the refresh token cookie.

## Standardized Responses (`Result<T>`)

To provide consistent API responses and eliminate the use of exceptions for expected control flow, the application uses a `Result<T>` pattern.

### Success Response
When a request is successful, you will typically receive a `200 OK` or `201 Created` with the payload:
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe"
}
```

### Error Responses
When a business rule is violated (e.g., trying to add a friend who is already a friend), the Application layer returns a failed `Result`. The API controller translates this into an appropriate HTTP status code (e.g., `400 Bad Request` or `404 Not Found`) and returns a Problem Details formatted JSON:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "You already are friends."
}
```

## Real-Time Messaging (SignalR)

The chat functionality relies on ASP.NET Core SignalR.
- **Hub Endpoint:** `/hubs/chat`
- **Authentication:** The client must pass the JWT token (typically via query string `?access_token=...` when establishing the WebSocket connection).
- **Methods:**
  - `SendMessage(int receiverId, string content)`
  - Listen for `ReceiveMessage` on the client.
