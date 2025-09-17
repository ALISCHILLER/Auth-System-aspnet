# 🧱 AuthSystem.Domain — Domain Layer Structure (v2025)

> Fully DDD & Clean Architecture Compliant — 100% Infrastructure-Free

---

## 📁 Root

- `AuthSystem.Domain.csproj`
- `map.docx` (optional)
- `structure.txt` (this file)

---

## 📁 Common/

### 📁 Auditing/
- `IAuditableEntity.cs`
- `IFullyAuditableEntity.cs`
- `ISoftDeletableEntity.cs`

### 📁 Clock/
- `DomainClock.cs`
- `ISystemClock.cs`

### 📁 Entities/
- `BaseEntity.cs`
- `AggregateRoot.cs`
- `ValueObject.cs`

### 📁 Events/
- `IDomainEvent.cs`
- `IAsyncDomainEvent.cs`
- `DomainEventBase.cs`
- `DomainEventMetadata.cs`

### 📁 Exceptions/
- `DomainException.cs`
- `BusinessRuleValidationException.cs`
- `AggregateBusinessRuleValidationException.cs`
- `PolicyViolationException.cs`

### 📁 Extensions/
- `DateTimeExtensions.cs`
- `EmailExtensions.cs`
- `EnumerableExtensions.cs`
- `PhoneNumberExtensions.cs`
- `StringExtensions.cs`
- `UserStatusExtensions.cs`
- `ValidationExtensions.cs`
- `VerificationCodeExtensions.cs`

### 📁 Policies/
- `IPolicy.cs`
- `IAsyncPolicy.cs`
- `BasePolicy.cs`
- `PolicyResult.cs`
- `PolicyEvaluator.cs`

### 📁 Rules/
- `IBusinessRule.cs`
- `IAsyncBusinessRule.cs`
- `BusinessRuleBase.cs`
- `AsyncBusinessRuleBase.cs`
- `RuleResult.cs`

### 📁 Specifications/
- `ISpecification.cs`
- `BaseSpecification.cs`
- `CompositeSpecification.cs`
- `AndSpecification.cs`
- `OrSpecification.cs`
- `NotSpecification.cs`
- `ExpressionComposer.cs`
- `SpecificationExtensions.cs`

### 📁 Testing/
- `DomainTestBase.cs` *(optional — can be moved to test project)*

---

## 📁 Entities/

### 📁 AuditLog/
- `AuditLog.cs`
- `AuditLogEntry.cs`

### 📁 Authorization/Role/
#### 📁 Events/
- `RoleCreatedEvent.cs`
- `RoleUpdatedEvent.cs`
- `RoleDeletedEvent.cs`
- `RoleUndeletedEvent.cs`
- `RolePermissionAddedEvent.cs`
- `RolePermissionRemovedEvent.cs`

#### 📁 Rules/
- `RoleNameUniqueRule.cs`
- `RoleMustHavePermissionsRule.cs`
- `RoleNameMustBeValidRule.cs`
- `RoleDescriptionMustBeValidRule.cs`
- `RoleCannotHaveDuplicatePermissionsRule.cs`
- `SystemRoleCannotRemoveAdminPermissionRule.cs`
- `RoleCannotHaveDuplicateUsersRule.cs`
- `DefaultRoleCannotBeEmptyRule.cs`

#### 📁 Specifications/
- `RoleByNameSpecification.cs`
- `RoleByPermissionsSpecification.cs`

#### Files:
- `RolePermission.cs`
- `Role.cs`
- `UserRole.cs`

### 📁 UserAggregate/
#### 📁 Events/
- `UserRegisteredEvent.cs`
- `EmailVerifiedEvent.cs`
- `UserPasswordChangedEvent.cs`
- `TwoFactorEnabledEvent.cs`
- `TwoFactorDisabledEvent.cs`
- `UserLoggedInEvent.cs`
- `UserLoginFailedEvent.cs`
- `UserLockedEvent.cs`
- `UserUnlockedEvent.cs`
- `UserStatusChangedEvent.cs`
- `UserRoleAddedEvent.cs`
- `UserRoleRemovedEvent.cs`
- `UserRoleChangedEvent.cs`

#### 📁 Rules/
- `UserMustHaveValidNameRule.cs`
- `UserMustHaveValidEmailRule.cs`
- `UserMustHaveValidPhoneRule.cs`
- `UserMustHaveValidNationalCodeRule.cs`
- `UserMustHaveValidPasswordRule.cs`
- `UserEmailMustBeUniqueRule.cs`
- `UsernameMustBeValidRule.cs`
- `UserCannotLoginWhenLockedRule.cs`
- `UserStatusMustBeActiveRule.cs`
- `UserRoleCannotBeDuplicatedRule.cs`

#### 📁 Specifications/
- `ActiveUserSpecification.cs`
- `UserByEmailSpecification.cs`
- `UserByUsernameSpecification.cs`

#### Files:
- `User.cs`

---

## 📁 Enums/

- `AuditLogLevel.cs`
- `AuthenticationMethod.cs`
- `CodeFormat.cs`
- `DeviceType.cs`
- `HashAlgorithm.cs`
- `IpVersion.cs`
- `PermissionType.cs`
- `PhoneType.cs`
- `TokenType.cs`
- `TwoFactorErrorType.cs`
- `UserStatus.cs`
- `VerificationCodeType.cs`

---

## 📁 Exceptions/

- `DuplicateEmailException.cs`
- `InvalidAuditLogEntryException.cs`
- `InvalidAuditLogException.cs`
- `InvalidEmailException.cs`
- `InvalidIpAddressException.cs`
- `InvalidNationalCodeException.cs`
- `InvalidPasswordException.cs`
- `InvalidPhoneNumberException.cs`
- `InvalidTokenException.cs`
- `InvalidTwoFactorSecretKeyException.cs`
- `InvalidUserException.cs`
- `InvalidUserRoleException.cs`
- `InvalidVerificationCodeException.cs`
- `RateLimitExceededException.cs`
- `TwoFactorException.cs`
- `UnauthorizedAccessException.cs`
- `UserNotFoundException.cs`

---

## 📁 Factories/

- `UserFactory.cs`
- `SecurityFactory.cs`

---

## 📁 ValueObjects/

- `Email.cs`
- `IpAddress.cs`
- `NationalCode.cs`
- `OtpCode.cs`
- `PasswordHash.cs`
- `PhoneNumber.cs`
- `TokenValue.cs`
- `TwoFactorSecretKey.cs`
- `UserAgent.cs`
- `VerificationCode.cs`

---

## 🧹 Clean Architecture Notes

- ❌ No MediatR Behaviors → Moved to **Application Layer**
- ❌ No Mocks → Moved to **Test Projects**
- ❌ No Service Contracts (Email, Crypto, etc.) → Moved to **Application/Infrastructure**
- ✅ Domain Events are **aggregate-scoped** — Common folder holds only abstractions
- ✅ No `IUserRepository` in Domain — Ports belong in **Application Layer**

---