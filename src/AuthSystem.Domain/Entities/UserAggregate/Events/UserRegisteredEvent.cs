using System;
using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted when a new user is registered.
/// </summary>
public sealed class UserRegisteredEvent : DomainEventBase
{
    public UserRegisteredEvent(
        Guid userId,
        Email? email,
        PasswordHash passwordHash,
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        DateTime? dateOfBirth,
        NationalCode? nationalCode,
        bool isEmailVerified,
        bool isPhoneVerified,
        bool isSocialLogin,
        UserStatus status)
    {
        UserId = userId;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        NationalCode = nationalCode;
        IsEmailVerified = isEmailVerified;
        IsPhoneVerified = isPhoneVerified;
        IsSocialLogin = isSocialLogin;
        Status = status;
    }

    public Guid UserId { get; }

    public Email? Email { get; }
    public PasswordHash PasswordHash { get; }

    public string FirstName { get; }

    public string LastName { get; }
    public PhoneNumber? PhoneNumber { get; }
    public DateTime? DateOfBirth { get; }
    public NationalCode? NationalCode { get; }
    public bool IsEmailVerified { get; }
    public bool IsPhoneVerified { get; }
    public bool IsSocialLogin { get; }
    public UserStatus Status { get; }
}