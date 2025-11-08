# Auth System Developer Portal

Welcome to the lightweight developer experience bundle for the Auth System platform. Use these artefacts to onboard quickly and integrate securely.

## 1. Documentation Hub

* **OpenAPI UI** – `https://{host}/swagger` provides versioned REST documentation with request/response examples and built-in JWT authentication support.
* **GraphQL Playground** – `https://{host}/graphql` (authenticated) offers an interactive explorer powered by Hot Chocolate. Load the schema SDL via the "Docs" panel to inspect field descriptions.
* **gRPC Descriptor** – Retrieve the compiled descriptor set by downloading `src/AuthSystem.Api/Grpc/security_events.proto` or call `dotnet grpc --proto src/AuthSystem.Api/Grpc/security_events.proto --csharp_out ./generated` to bootstrap SDKs.

## 2. API Collections & CLI Helpers

* **Postman Collection** – Import `docs/postman/AuthSystem.postman_collection.json` for curated REST requests covering login, audit queries, SCIM, and webhook testing. Environment variables capture host, tenant ID, and bearer tokens.
* **CLI Script** – `scripts/authsystem-cli.sh` wraps curl invocations for token issuance, audit export (REST or GraphQL), and webhook replay. Ideal for CI smoke tests and manual diagnostics.

## 3. Multi-Protocol Guidance

| Protocol | Endpoint | Notes |
| --- | --- | --- |
| REST | `/api/v1/auth`, `/api/v1/audit` | JSON responses wrapped in the shared `ApiResponse<T>` envelope. |
| gRPC | `/grpc` | Use TLS + JWT for authentication. Supported method: `ListSecurityEvents`. |
| GraphQL | `/graphql` | Query `securityEvents(page: 1, tenantId: "demo") { items { id eventType occurredAtUtc } }`. |
| SCIM | `/api/v1/scim/v2/Users` | Supports SCIM list, read, create, replace, patch, and delete operations. |
| Webhooks | Configured under `SecurityWebhooks` | Receives POST payloads; verify the `X-Signature` header. |
| SignalR | `/hubs/security-events` | Real-time push of security events for observability dashboards. |

## 4. Tenant & Rate Limiting

* Always send the `X-Tenant-Id` header. The platform partitions rate limits and security analytics per tenant.
* JWTs now embed the `tenant` claim. Combine with the header for cross-environment safety nets.
* For SSO-driven logins, include `X-External-Login: true` to trigger JIT provisioning and bypass password verification.

## 5. Audit Streaming Recipes

```bash
# REST pagination
./scripts/authsystem-cli.sh audit-rest --tenant demo --page 1

# GraphQL projection
./scripts/authsystem-cli.sh audit-graphql --tenant demo --fields "id eventType metadata"

# gRPC tail (requires grpcurl)
grpcurl -H "authorization: Bearer $TOKEN" -d '{"page":1,"page_size":50}' \
  $HOST:5001 authsystem.security.SecurityEvents/ListSecurityEvents
```

## 6. Webhook Registration Checklist

1. Enable the `SecurityWebhooks` section and provide at least one subscription.
2. Share the generated `X-Signature` secret with the receiving system.
3. Use the Postman collection or CLI to trigger a login and confirm webhook delivery.
4. Monitor failure logs under the `Security` logger category for retries or signature mismatches.

## 7. Roadmap Notes

* Additional SDK generators (TypeScript, Kotlin) will be added when Hot Chocolate federation stabilises.
* SCIM CRUD operations and webhook broadcasting are now live; upcoming work focuses on richer SDK generation and passkey support.

Happy building! 🎉