# Configuration Guide

This document summarises the configuration knobs introduced for the infrastructure services so the application can run in different environments confidently.

## Email (`Email` section)

| Key | Description |
| --- | --- |
| `Provider` | `smtp` to enable the SMTP sender, or `noop` to disable real delivery. |
| `From` / `DisplayName` | Sender address and optional display name used for outgoing messages. |
| `Host` / `Port` | SMTP server location. |
| `EnableSsl` | Enables STARTTLS/SSL when `true`. |
| `Username` / `Password` | Credentials if the SMTP relay requires authentication. |

For local development the sample `appsettings.Development.json` targets a Mailtrap sandbox. Replace the username/password placeholders with the credentials from your Mailtrap inbox before running the API locally.

## SMS (`Sms` section)

| Key | Description |
| --- | --- |
| `Provider` | `http` to enable the HTTP provider, or `noop` to disable delivery. |
| `Endpoint` | Relative or absolute URI of the SMS provider endpoint. |
| `ApiKey` | Optional API key sent with `X-Api-Key`. |
| `FromNumber` | Sender phone number when required by the provider. |
| `Headers` | Extra HTTP headers applied to every request (e.g. `X-Tenant`). |
| `TimeoutSeconds` | HTTP timeout for the SMS client. |
| `RetryCount` | Number of exponential back-off retries for transient failures. |

In integration tests or staging environments you can point the endpoint to a fake provider and assert on the outbound requests.

## API Rate Limiting (`RateLimiting` section)

| Policy | Purpose |
| --- | --- |
| `Global` | Applies a global cap to every incoming request regardless of route. |
| `AuthLogin` | Protects the login endpoint from brute-force attempts. |
| `AuthRefresh` | Controls refresh token churn to prevent token spraying. |
| `AuthLogout` | Keeps logout activity in check while remaining generous to legitimate traffic. |
| `TwoFactorRequest` | Limits how often a user can request a 2FA code to prevent spamming the delivery channel. |
| `TwoFactorVerify` | Throttles verification attempts to guard against brute-force code guessing. |

Each policy accepts `PermitLimit`, `WindowSeconds`, and `QueueLimit`. Update the values per environment to balance protection and usability.

## Telemetry (`Telemetry` section)

`EnableTracing` toggles OpenTelemetry tracing (ASP.NET Core + `HttpClient` instrumentation). `EnableMetrics` controls runtime, ASP.NET Core, and HTTP metrics. Configure exporters in `Program.cs` (console by default) or via additional OpenTelemetry extensions.

## Security Webhooks (`SecurityWebhooks` section)

| Key | Description |
| --- | --- |
| `Enabled` | Enables outbound webhook delivery when `true`. |
| `Subscriptions[].Name` | Friendly identifier that appears in logs. |
| `Subscriptions[].Url` | Absolute HTTPS endpoint that receives JSON payloads. |
| `Subscriptions[].EventTypes` | Optional allow-list (e.g. `Login`, `Logout`, `TwoFactorFailed`). Empty means all events. |
| `Subscriptions[].Secret` | Shared secret used to generate an `X-Signature` HMAC header. |
| `Subscriptions[].Headers` | Additional static headers required by the consumer (e.g. `X-Webhook-Provider`). |

Webhook payloads include the event identifier, tenant, IP address, metadata dictionary, and UTC timestamp. Configure your observability platform (Seq, ELK, etc.) to accept these calls and verify the signature when `Secret` is set.

## Security (`Security` section)

Currently this section documents the active password hasher implementation (`AspNetIdentity`). The infrastructure registers `AspNetPasswordHasher` and `SystemDateTimeProvider` so application code can hash passwords and evaluate expirations without taking direct framework dependencies.

## Developer Integrations

- **gRPC** – The `SecurityEvents` service exposes the same audit feed over gRPC. Point Postman/Insomnia or your internal SDKs to `/grpc` and use the generated proto at `src/AuthSystem.Api/Grpc/security_events.proto`.
- **GraphQL** – Browse to `/graphql` (secured) to run GraphQL queries. The root `Query` type exposes `securityEvents` with paging, filtering, and metadata fields.
- **SignalR** – Connect to `/hubs/security-events` with an authenticated client to receive push notifications for security events in real time.
- **SCIM** – Enterprise directories can integrate with `/api/scim/v2/Users` to enumerate or resolve users. Responses comply with SCIM v2 list and user schemas.
- **Developer Portal Assets** – See `docs/devportal/README.md` for SDK generation tips, Postman collections, and onboarding checklists.

## Health checks

`AddInfrastructure` wires a resilient `HttpClient` for SMS delivery that honours the configured timeout and retry count. If you introduce a production provider, consider extending the health checks to cover the external dependency and monitor latency.