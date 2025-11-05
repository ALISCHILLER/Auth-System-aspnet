# AuthSystem.Domain Structure

This document outlines the organization of the domain layer, highlighting the core building blocks that make up the DDD-oriented model.

## Directory Overview

```
src/AuthSystem.Domain/
├─ Common/
│  ├─ Abstractions/
│  │  ├─ IDomainEvent.cs
│  │  ├─ IHasDomainEvents.cs
│  │  ├─ IBusinessRule.cs
│  │  ├─ IAsyncBusinessRule.cs
│  │  └─ ISpecification.cs
│  ├─ Base/
│  │  ├─ Entity.cs
│  │  ├─ AggregateRoot.cs
│  │  └─ ValueObject.cs
│  ├─ Clock/
│  │  ├─ IDomainClock.cs
│  │  ├─ DomainClock.cs
│  │  └─ SystemDomainClock.cs
│  ├─ Events/
│  │  └─ DomainEvent.cs
│  ├─ Errors/
│  │  └─ DomainError.cs
│  ├─ Exceptions/
│  │  └─ DomainException.cs
│  └─ Rules/
│     └─ BusinessRuleBase.cs
├─ Enums/
│  └─ PermissionType.cs
├─ ValueObjects/
│  ├─ Email.cs
│  ├─ PhoneNumber.cs
│  ├─ PasswordHash.cs
│  ├─ UserId.cs
│  ├─ RoleId.cs
│  └─ (additional supporting VOs)
├─ Entities/
│  ├─ UserAggregate/
│  │  ├─ User.cs
│  │  ├─ Events/
│  │  │  ├─ UserRegisteredEvent.cs
│  │  │  ├─ UserEmailChangedEvent.cs
│  │  │  └─ UserTwoFactorToggledEvent.cs
│  │  ├─ Policies/
│  │  │  └─ UserPolicies.cs
│  │  └─ Services/
│  │     └─ IUserDomainService.cs
│  └─ Authorization/
│     ├─ Role/
│     │  ├─ Role.cs
│     │  ├─ RolePermission.cs
│     │  └─ Events/
│     │     └─ RoleCreatedEvent.cs
│     └─ UserRole.cs
├─ Factories/
│  ├─ UserFactory.cs
│  └─ RoleFactory.cs
└─ Docs/
   └─ DOMAIN_STRUCTURE.md
```

## Key Design Points

- **Common/Base** defines the foundational types (`Entity`, `AggregateRoot`, `ValueObject`) that implement equality, rule checking, and domain-event tracking.
- **Common/Abstractions** contains interfaces for domain events, business rules, and specifications, enabling loose coupling across aggregates.
- **Common/Clock** exposes an overridable clock abstraction (`IDomainClock`) alongside the default `SystemDomainClock` and a simple static `DomainClock` accessor used within the model.
- **Common/Events** provides the `DomainEvent` base class leveraged by aggregate events. Events surface through the `IHasDomainEvents` contract and are dispatched by infrastructure.
- **ValueObjects** encapsulate immutable concepts such as `Email`, `PhoneNumber`, and identifier wrappers (`UserId`, `RoleId`). Validation and normalization logic lives inside the VOs themselves.
- **Entities/UserAggregate** contains the `User` aggregate root plus related events, policies, and services capturing user-specific behavior (registration, email changes, two-factor toggling, etc.).
- **Entities/Authorization** models roles and user-role assignments, including domain events for role lifecycle operations.
- **Factories** offer convenience helpers for constructing aggregates with the correct invariants (e.g., provisioning users or roles with predefined permissions).

Each aggregate raises domain events through `AggregateRoot`’s `ApplyRaise` helper. Events are collected via `IDomainEventCollector` and dispatched through infrastructure adapters, keeping the domain free from external dependencies.