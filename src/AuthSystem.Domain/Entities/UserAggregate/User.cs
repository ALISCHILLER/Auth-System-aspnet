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
/// ریشه تجمع کاربر
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

        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        NationalCode = nationalCode;
        IsEmailVerified = isEmailVerified && email is not null;
        IsPhoneVerified = isPhoneVerified && phoneNumber is not null;
        IsSocialLogin = isSocialLogin;
        Status = UserStatus.Pending;
        AccessFailedCount = 0;

        ApplyRaise(new UserRegisteredEvent(id, email?.Value, firstName, lastName));
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

    public IReadOnlyDictionary<Guid, string> Roles => _roles.ToDictionary(
        static pair => pair.Key,
        static pair => pair.Value.RoleName);

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

        PasswordHash = passwordHash;
        ApplyRaise(new UserPasswordChangedEvent(Id));
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
        AccessFailedCount++;
        MarkAsUpdated();
        ApplyRaise(new UserLoginFailedEvent(Id, reason, AccessFailedCount));
    }

    public void Lock(TimeSpan? duration = null)
    {
        LockoutEnd = DomainClock.Instance.UtcNow.Add(duration ?? TimeSpan.FromMinutes(15));
        ApplyRaise(new UserLockedEvent(Id, LockoutEnd));
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

        var previous = Status;
        Status = status;
        ApplyRaise(new UserStatusChangedEvent(Id, previous, status));
    }

    public void AddRole(Guid roleId, string roleName)
    {
        CheckRule(new UserRoleCannotBeDuplicatedRule(_roles.Keys, roleId));
        var previousRoles = _roles.Values.Select(r => r.RoleName).ToArray();

        _roles[roleId] = new UserRoleInfo(roleName, DomainClock.Instance.UtcNow);
        MarkAsUpdated();

        ApplyRaise(new UserRoleAddedEvent(Id, roleId, roleName));
        PublishRolesChangedEvent(previousRoles);
    }

    public void RemoveRole(Guid roleId)
    {
        if (!_roles.TryGetValue(roleId, out var role))
        {
            throw InvalidUserRoleException.ForRoleAssignmentNotFound(roleId);
        }

        var previousRoles = _roles.Values.Select(r => r.RoleName).ToArray();

        _roles.Remove(roleId);
        MarkAsUpdated();

        ApplyRaise(new UserRoleRemovedEvent(Id, roleId, role.RoleName));
        PublishRolesChangedEvent(previousRoles);
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

    private void PublishRolesChangedEvent(IReadOnlyCollection<string> previousRoles)
    {
        var currentRoles = _roles.Values.Select(r => r.RoleName).ToArray();
        ApplyRaise(new UserRoleChangedEvent(Id, previousRoles, currentRoles));
    }

    private sealed record UserRoleInfo(string RoleName, DateTime AssignedAt);

    private void On(UserRegisteredEvent @event)
    {
        Status = UserStatus.Pending;
    }

    private void On(EmailVerifiedEvent @event)
    {
        IsEmailVerified = true;
        MarkAsUpdated();
    }

    private void On(UserPasswordChangedEvent @event)
    {
        ResetAccessFailedCount();
    }

    private void On(TwoFactorEnabledEvent @event)
    {
        TwoFactorSecretKey = @event.SecretKey;
        IsTwoFactorEnabled = true;
        MarkAsUpdated();
    }

    private void On(TwoFactorDisabledEvent @event)
    {
        TwoFactorSecretKey = null;
        IsTwoFactorEnabled = false;
        MarkAsUpdated();
    }

    private void On(UserLoggedInEvent @event)
    {
        LastLoginAt = DomainClock.Instance.UtcNow;
        AccessFailedCount = 0;
        LockoutEnd = null;
        MarkAsUpdated();
    }

    private void On(UserLoginFailedEvent @event)
    {
        if (AccessFailedCount >= 5)
        {
            Lock(TimeSpan.FromMinutes(15));
        }
    }

    private void On(UserLockedEvent @event)
    {
        LockoutEnd = @event.LockoutEnd;
        MarkAsUpdated();
    }

    private void On(UserUnlockedEvent @event)
    {
        LockoutEnd = null;
        AccessFailedCount = 0;
        MarkAsUpdated();
    }

    private void On(UserStatusChangedEvent @event)
    {
        Status = @event.NewStatus;
        MarkAsUpdated();
    }

    private void On(UserRoleAddedEvent @event)
    {
    }

    private void On(UserRoleRemovedEvent @event)
    {
    }

    private void On(UserRoleChangedEvent @event)
    {
    }
}