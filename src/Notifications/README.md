# Notifications Service

`Notifications.API` accepts notifications from internal producers (the main API) and delivers them to users. It stores everything in its own **PostgreSQL** database, so a notification survives crashes, restarts and offline recipients. Today it delivers over **SignalR**. Delivery channels are pluggable, so FCM, email or others can be added beside it.

## How delivery works

```
POST /notifications/push  ──▶  one transaction:
  (X-Api-Key)                    notifications  (event_id is unique → duplicates are no-ops)
                                 deliveries     (one row per recipient × enabled channel)
                               ◀── 202 Accepted   (nothing is sent during the request)

DeliveryWorker (one per channel)
  claims due rows (FOR UPDATE SKIP LOCKED + lease) → channel.SendAsync → records the outcome

SignalR channel
  recipient offline → Parked     → replayed when their first connection opens
  recipient online  → ReceiveNotification → AwaitingAck → client calls Acknowledge → Delivered
                                             no ack within AckTimeout → sent again
```

| Status | Meaning |
| --- | --- |
| `Pending` | Due at `next_attempt_at` (immediately, or after a retry backoff). |
| `Processing` | Claimed by a worker. If the worker dies, the row is re-claimed when the lease (`next_attempt_at`) expires. |
| `AwaitingAck` | Sent. Waiting for the client's ack until `next_attempt_at`, then re-sent. |
| `Parked` | The recipient isn't connected. This doesn't count as an attempt. Resumed on reconnect. |
| `Delivered` / `Failed` / `Expired` | Terminal. `Failed` has the reason in `last_error`. It is reached after `MaxAttempts` sends without an ack, after repeated transient errors, or on a permanent error. |

**Guarantees:** delivery is **at-least-once**. A client may receive the same notification more than once, for example when its ack is lost or arrives late. **Clients must de-duplicate by `eventId`.**

## Producer API

`POST /notifications/push` with the header `X-Api-Key: <Intake:ApiKey>`:

```json
{
  "eventId": "friend-request:123",
  "title": "New friend request",
  "body": "John Doe sent you a friend request",
  "recipients": ["42", "77"],
  "data": { "requesterId": 7 },
  "createdAt": "2026-10-10T12:00:00Z",
  "expiresAt": "2026-10-11T12:00:00Z"
}
```

- `eventId`: the producer's idempotency key, up to 128 characters. Retrying with the same id is safe: the response is `202` with `"duplicate": true` and nothing new is created. Use a stable id, such as the entity id plus the event type, so retries reuse it.
- `recipients`: user ids as they appear in the JWT's `nameid` claim. 1–1000 per request; duplicates are ignored.
- `data`: optional JSON, passed to clients unchanged.
- `expiresAt`: optional. Undelivered notifications are dropped after this time instead of being sent late.

Responses:
- `202 { "eventId": "...", "duplicate": false }`
- `400` with a ValidationProblem body
- `401` when the API key is missing or wrong

## Client contract (SignalR)

- **Hub:** `/hubs/notifications`. Authenticate with the same JWT the main API issues: `accessTokenFactory` / `?access_token=`.
- **Server → client:** `ReceiveNotification(message)`, where `message` is `{ eventId, title, body, data, createdAt }`.
- **Client → server:** `Acknowledge(eventId)`. Call it after handling each notification. A user can only acknowledge their own deliveries. Acknowledging an unknown or already-acknowledged id does nothing.

```ts
connection.on('ReceiveNotification', async (n) => {
  if (!seen.has(n.eventId)) { seen.add(n.eventId); show(n); }
  await connection.invoke('Acknowledge', n.eventId);   // ack duplicates too
});
```

## Adding a channel

1. Implement `INotificationChannel.SendAsync`. Return:
   - `Delivered` when the provider confirms,
   - `AwaitingAck` when only the client can confirm,
   - `RecipientUnavailable` to park,
   - `Transient(error, retryAfter)` or `Permanent(error)` for failures.

   An exception thrown from `SendAsync` counts as transient.
2. Register it in `DependencyInjection.AddDelivery`: `services.AddNotificationChannel<FcmChannel>("fcm")`.

New notifications then fan out to every enabled channel, and each channel gets its own worker, retries and options. To run a channel's worker on a different host, set `Delivery:Channels:<name>:WorkerEnabled=false` here and `true` there. The SignalR worker must stay in the hub's process until a Redis backplane is added.

## Configuration

| Key | Default | Notes |
| --- | --- | --- |
| `ConnectionStrings:NotificationsConnection` | none (required) | PostgreSQL connection string |
| `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key` | none (required) | Must match the main API's |
| `Intake:ApiKey` | none (required) | Shared secret for producers |
| `Cors:AllowedOrigins` | `[]` | SPA origins allowed to open the hub |
| `Delivery:Channels:<name>:Enabled` / `WorkerEnabled` | `true` / `true` | See "Adding a channel" |
| `Delivery:Channels:<name>:MaxAttempts` | `8` | Sends before `Failed` |
| `Delivery:Channels:<name>:AckTimeout` | `00:00:30` | Time to wait for an ack before re-sending |
| `Delivery:Channels:<name>:BackoffBase` / `BackoffCap` | `00:00:05` / `00:10:00` | Exponential retry backoff with ±20% jitter |
| `Delivery:Channels:<name>:BatchSize` / `MaxConcurrency` / `PollInterval` / `LeaseDuration` | `50` / `4` / `00:00:02` / `00:01:00` | Worker tuning |
| `Delivery:Cleanup:Interval` / `MaxParkDuration` / `Retention` | `01:00:00` / `7.00:00:00` / `7.00:00:00` | Expire long-parked rows; delete finished notifications |

The project shares the main API's **user-secrets store** (same `UserSecretsId`), so the `Jwt:*` values already set for the API apply here too. Add the service-specific values once:

```bash
dotnet user-secrets set "Intake:ApiKey" "<random secret>" --project src/Notifications/Notifications.API
dotnet user-secrets set "ConnectionStrings:NotificationsConnection" "Host=localhost;Port=5432;Database=notifications;Username=notifications;Password=Password123!" --project src/Notifications/Notifications.API
```

## Running

```bash
docker compose up -d notifications-db                       # PostgreSQL on localhost:5432
dotnet run --project src/Notifications/Notifications.API    # migrations are applied at startup in Development
# or the containerised service on http://localhost:5010:
docker compose up -d --build notifications-db notifications-api
```

Health checks: `/health/live` and `/health/ready` (the ready check includes the database). Metrics are published on the `Notifications.Delivery` meter:
- `notifications.delivery.transitions`, tagged by channel and status
- `notifications.delivery.latency`

## Migrations

```bash
dotnet ef migrations add <Name> --project src/Notifications/Notifications.API --startup-project src/Notifications/Notifications.API --output-dir Data/Migrations
dotnet ef database update --project src/Notifications/Notifications.API --startup-project src/Notifications/Notifications.API --connection "<connection string>"
```

Design-time tooling uses `DesignTimeDbContextFactory` and a placeholder connection string, so pass `--connection` to commands that touch a database. Add `// <auto-generated />` at the top of each new migration file, as the API's migrations have, so analyzers skip it.

## Current limitations

- Presence is in memory, so the service runs as a **single instance**. Scaling out needs a Redis backplane and shared presence.
- The main API doesn't call this service yet. It still pushes chat notifications itself through `MessageNotifier`.
