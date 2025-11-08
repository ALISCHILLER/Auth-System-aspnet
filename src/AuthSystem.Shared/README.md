# AuthSystem.Shared

This project contains cross-cutting contracts and DTOs that can be consumed by multiple layers without depending on infrastructure concerns.

- `DTOs/` hosts request/response primitives such as `PagedRequest` and `PagedResult` that can be re-used by HTTP and background entry points.
- `Contracts/` exposes the `OperationResult` and `ApiResponse` envelopes so commands, queries, and API controllers can return a uniform shape.
- `CommonExceptions/` centralises exception types that bubble up through multiple layers.

As additional shared artefacts emerge (for example, user-facing contracts for integration events) extend this project instead of duplicating definitions across solutions.