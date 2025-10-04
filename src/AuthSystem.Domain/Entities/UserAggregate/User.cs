using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common.Clock;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Entities.UserAggregate.Events;
using AuthSystem.Domain.Entities.UserAggregate.Rules;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate;

/// <summary>
/// Aggregate root representing a user.
/// </summary>
public sealed class User : AggregateRoot<Guid>
{
    private readonly Dictionary<Guid, UserRoleInfo> _roles = new();
    private readonly Dictionary<string, string> _socialLogins = new(StringComparer.OrdinalIgnoreCase);

    private User()
    {
    }

    public User(
        Guid id,
        Email? email,
        PasswordHash passwordHash,
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber = null,
        DateTime? dateOfBirth = null,
        NationalCode? nationalCode = null,
        bool isEmailVerified = false,
        bool isPhoneVerified = false,
        bool isSocialLogin = false) : base(id)
    {
        if (email is null && phoneNumber is null)
        {
            throw InvalidUserException.ForMissingRequiredData();
        }

        CheckRule(new UserMustHaveValidNameRule(firstName, lastName));
        CheckRule(new UserMustHaveValidPasswordRule(passwordHash));

        if (email is not null)
        {
            CheckRule(new UserMustHaveValidEmailRule(email.Value));
        }

        if (phoneNumber is not null)
        {
            CheckRule(new UserMustHaveValidPhoneRule(phoneNumber.Value));
        }

        if (nationalCode is not null)
        {
            CheckRule(new UserMustHaveValidNationalCodeRule(nationalCode.Value));
        }

        ApplyRaise(new UserRegisteredEvent(
            id,
            email,
            passwordHash,
            firstName,
            lastName,
            phoneNumber,
            dateOfBirth,
            nationalCode,
            isEmailVerified && email is not null,
            isPhoneVerified && phoneNumber is not null,
            isSocialLogin,
            UserStatus.Pending));
    }

    public Email? Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; } = default!;

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string? Username { get; private set; }

    public PhoneNumber? PhoneNumber { get; private set; }

    public DateTime? DateOfBirth { get; private set; }

    public NationalCode? NationalCode { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public bool IsPhoneVerified { get; private set; }

    public bool IsTwoFactorEnabled { get; private set; }

    public TwoFactorSecretKey? TwoFactorSecretKey { get; private set; }

    public bool IsSocialLogin { get; private set; }

    public UserStatus Status { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public int AccessFailedCount { get; private set; }

    public DateTime? LockoutEnd { get; private set; }

    public IReadOnlyDictionary<Guid, string> Roles => _roles.ToDictionary(static pair => pair.Key, static pair => pair.Value.RoleName);

    public IReadOnlyDictionary<string, string> SocialLogins => _socialLogins;

    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DomainClock.Instance.UtcNow;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public void SetUsername(string username)
    {
        CheckRule(new UsernameMustBeValidRule(username));

        Username = username;
        MarkAsUpdated();
    }

    public void ChangeEmail(Email email)
    {
        Email = email ?? throw InvalidUserException.ForInvalidEmail(string.Empty);
        IsEmailVerified = false;
        MarkAsUpdated();
    }

    public void VerifyEmail()
    {
        if (Email is null)
        {
            throw InvalidUserException.ForInvalidEmail(string.Empty);
        }

        ApplyRaise(new EmailVerifiedEvent(Id, Email.Value));
    }

    public void VerifyPhone()
    {
        if (PhoneNumber is null)
        {
            throw InvalidUserException.ForInvalidPhoneNumber(string.Empty);
        }

        IsPhoneVerified = true;
        MarkAsUpdated();
    }

    public void ChangePassword(PasswordHash passwordHash)
    {
        CheckRule(new UserMustHaveValidPasswordRule(passwordHash));

        ApplyRaise(new UserPasswordChangedEvent(Id, passwordHash));
    }

    public void EnableTwoFactorAuthentication(TwoFactorSecretKey secretKey)
    {
        if (secretKey is null)
        {
            throw new InvalidTwoFactorSecretKeyException("کلید احراز هویت دو عاملی نامعتبر است");
        }

        ApplyRaise(new TwoFactorEnabledEvent(Id, secretKey));
    }

    public void DisableTwoFactorAuthentication()
    {
        if (!IsTwoFactorEnabled)
        {
            return;
        }

        ApplyRaise(new TwoFactorDisabledEvent(Id));
    }

    public void RegisterLoginSuccess(IpAddress? ipAddress = null, UserAgent? userAgent = null)
    {
        CheckRule(new UserCannotLoginWhenLockedRule(LockoutEnd));
        CheckRule(new UserStatusMustBeActiveRule(Status));

        ApplyRaise(new UserLoggedInEvent(Id, ipAddress, userAgent));
    }

    public void RegisterLoginFailure(string reason)
    {
        var newAttemptCount = AccessFailedCount + 1;
        ApplyRaise(new UserLoginFailedEvent(Id, reason, newAttemptCount));
    }

    public void Lock(TimeSpan? duration = null)
    {
        var now = DomainClock.Instance.UtcNow;
        var proposedEnd = duration.HasValue ? now.Add(duration.Value) : now.AddMinutes(15);
        if (LockoutEnd.HasValue && LockoutEnd.Value > proposedEnd)
        {
            proposedEnd = LockoutEnd.Value;
        }

        ApplyRaise(new UserLockedEvent(Id, proposedEnd));
    }

    public void Unlock()
    {
        if (!IsLocked && AccessFailedCount == 0)
        {
            return;
        }

        ApplyRaise(new UserUnlockedEvent(Id));
    }

    public void SetStatus(UserStatus status)
    {
        if (Status == status)
        {
            return;
        }
        ApplyRaise(new UserStatusChangedEvent(Id, Status, status));
    }

    public void AddRole(Guid roleId, string roleName)
    {
        CheckRule(new UserRoleCannotBeDuplicatedRule(_roles.Keys, roleId));
        var previousRoles = _roles.Values.Select(r => r.RoleName).ToArray();

       

        ApplyRaise(new UserRoleAddedEvent(Id, roleId, roleName));
        var currentRoles = _roles.Values.Select(r => r.RoleName).ToArray();
        ApplyRaise(new UserRoleChangedEvent(Id, previousRoles, currentRoles));
    }

    public void RemoveRole(Guid roleId)
    {
        if (!_roles.TryGetValue(roleId, out var role))
        {
            throw InvalidUserRoleException.ForRoleAssignmentNotFound(roleId);
        }

        var previousRoles = _roles.Values.Select(r => r.RoleName).ToArray();

        

        ApplyRaise(new UserRoleRemovedEvent(Id, roleId, role.RoleName));
        var currentRoles = _roles.Values.Select(r => r.RoleName).ToArray();
        ApplyRaise(new UserRoleChangedEvent(Id, previousRoles, currentRoles));
    }

    public bool HasRole(Guid roleId) => _roles.ContainsKey(roleId);

    public void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
        MarkAsUpdated();
    }

    public void AddSocialLogin(string provider, string providerUserId)
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new ArgumentException("نام ارائه‌دهنده نمی‌تواند خالی باشد", nameof(provider));
        }

        if (string.IsNullOrWhiteSpace(providerUserId))
        {
            throw new ArgumentException("شناسه کاربر در شبکه اجتماعی نمی‌تواند خالی باشد", nameof(providerUserId));
        }

        _socialLogins[provider] = providerUserId;
        MarkAsUpdated();
    }


    private sealed record UserRoleInfo(string RoleName, DateTime AssignedAt);

    private void On(UserRegisteredEvent @event)
    {
        Id = @event.UserId;
        Email = @event.Email;
        PasswordHash = @event.PasswordHash;
        FirstName = @event.FirstName;
        LastName = @event.LastName;
        PhoneNumber = @event.PhoneNumber;
        DateOfBirth = @event.DateOfBirth;
        NationalCode = @event.NationalCode;
        IsEmailVerified = @event.IsEmailVerified;
        IsPhoneVerified = @event.IsPhoneVerified;
        IsSocialLogin = @event.IsSocialLogin;
        Status = @event.Status;
        IsTwoFactorEnabled = false;
        TwoFactorSecretKey = null;
        AccessFailedCount = 0;
        LockoutEnd = null;
        LastLoginAt = null;
        _roles.Clear();
        _socialLogins.Clear();
        MarkAsCreated(occurredOn: @event.OccurredOn);
    }

    private void On(EmailVerifiedEvent @event)
    {
        IsEmailVerified = true;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserPasswordChangedEvent @event)
    {
        PasswordHash = @event.PasswordHash;
        AccessFailedCount = 0;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(TwoFactorEnabledEvent @event)
    {
        TwoFactorSecretKey = @event.SecretKey;
        IsTwoFactorEnabled = true;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(TwoFactorDisabledEvent @event)
    {
        TwoFactorSecretKey = null;
        IsTwoFactorEnabled = false;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserLoggedInEvent @event)
    {
        LastLoginAt = @event.OccurredOn;
        AccessFailedCount = 0;
        LockoutEnd = null;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserLoginFailedEvent @event)
    {
        AccessFailedCount = @event.FailedAttempts;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
        if (AccessFailedCount >= 5)
        {
            Lock(TimeSpan.FromMinutes(15));
        }
    }

    private void On(UserLockedEvent @event)
    {
        LockoutEnd = @event.LockoutEnd;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserUnlockedEvent @event)
    {
        LockoutEnd = null;
        AccessFailedCount = 0;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserStatusChangedEvent @event)
    {
        Status = @event.NewStatus;
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserRoleAddedEvent @event)
    {
        _roles[@event.RoleId] = new UserRoleInfo(@event.RoleName, @event.OccurredOn);
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserRoleRemovedEvent @event)
    {
        _roles.Remove(@event.RoleId);
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }

    private void On(UserRoleChangedEvent @event)
    {
        MarkAsUpdated(occurredOn: @event.OccurredOn);
    }
}