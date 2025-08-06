using AuthSystem.Domain.Common;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Events;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Extensions;
using AuthSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت اصلی کاربر — یک مدل دامنه غنی (Rich Domain Model)
/// این کلاس تمام ویژگی‌ها و رفتارهای دامنه‌ای کاربر را تعریف می‌کند.
/// </summary>
public class User : BaseEntity
{
    #region Fields (فیلدهای پشتیبان)
    private string _userName;
    private Email _emailAddress;
    private string _passwordHash;
    private PhoneNumber _phoneNumber;
    private NationalCode _nationalCode;
    private string? _profileImageUrl;
    private Gender _gender = Gender.Unknown; ;
    #endregion

    #region Properties (ویژگی‌های خواندنی)
    public string UserName => _userName;
    public string EmailAddress => _emailAddress.Value;
    public bool EmailConfirmed { get; private set; }
    public string PhoneNumberValue => _phoneNumber.Value;
    public bool PhoneNumberConfirmed { get; private set; }
    public string NationalCodeValue => _nationalCode.Value;
    public string PasswordHash => _passwordHash;
    public string? ProfileImageUrl => _profileImageUrl;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }
    public Gender Gender => _gender;
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutEnd { get; private set; }
    public DateTime? PasswordExpiresAt { get; private set; }
    #endregion

    #region Navigation Properties (روابط)
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    public ICollection<LoginHistory> LoginHistories { get; private set; } = new List<LoginHistory>();
    public ICollection<UserDevice> UserDevices { get; private set; } = new List<UserDevice>();
    #endregion

    #region Constructors
    // سازنده پیش‌فرض فقط برای EF Core
    private User() { }
    #endregion

    #region Factory Method
    /// <summary>
    /// ساخت یک کاربر جدید
    /// </summary>
    public static User Create(string userName, string email, string passwordHash,
        string phoneNumber, string nationalCode)
    {
        var user = new User
        {
            _userName = EnsureValidString(userName, nameof(userName)),
            _emailAddress = Email.Create(email),
            _passwordHash = passwordHash,
            _phoneNumber = PhoneNumber.Create(phoneNumber),
            _nationalCode = NationalCode.Create(nationalCode)
        };

        user.InitializeEntity();
        user.AddDomainEvent(new UserRegisteredEvent(user.Id, email, userName));
        return user;
    }
    #endregion

    #region Update Methods
    public void UpdateEmail(string email)
    {
        var newEmail = Email.Create(email);
        SetValueIfChanged(ref _emailAddress, newEmail, () =>
        {
            EmailConfirmed = false;
            AddDomainEvent(new EmailChangedEvent(Id, email));
        });
    }

    public void ConfirmEmail()
    {
        if (EmailConfirmed)
            throw new EmailAlreadyConfirmedException(Id);
        EmailConfirmed = true;
        AddDomainEvent(new EmailConfirmedEvent(Id));
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        var newPhone = PhoneNumber.Create(phoneNumber);
        SetValueIfChanged(ref _phoneNumber, newPhone, () =>
        {
            PhoneNumberConfirmed = false;
            AddDomainEvent(new PhoneNumberChangedEvent(Id, phoneNumber));
        });
    }

    public void ConfirmPhoneNumber()
    {
        if (PhoneNumberConfirmed)
            throw new PhoneNumberAlreadyConfirmedException(Id);
        PhoneNumberConfirmed = true;
        AddDomainEvent(new PhoneNumberConfirmedEvent(Id));
    }

    public void UpdateNationalCode(string nationalCode)
    {
        var newCode = NationalCode.Create(nationalCode);
        SetValueIfChanged(ref _nationalCode, newCode, () =>
        {
            AddDomainEvent(new NationalCodeChangedEvent(Id, nationalCode));
        });
    }

    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new InvalidPasswordException("هش رمز عبور نمی‌تواند خالی باشد");

        _passwordHash = passwordHash;
        AddDomainEvent(new PasswordChangedEvent(Id));
    }

    public bool VerifyPassword(string inputPasswordHash) => _passwordHash == inputPasswordHash;

    public bool IsPasswordExpired() =>
        PasswordExpiresAt.HasValue && DateTime.UtcNow > PasswordExpiresAt.Value;

    public void ExtendPasswordExpiration(int days)
    {
        PasswordExpiresAt = DateTime.UtcNow.AddDays(days);
        MarkAsUpdated();
    }

    public void LockAccount(TimeSpan? lockoutDuration = null)
    {
        if (!IsActive) return;

        IsActive = false;
        LockoutEnd = lockoutDuration.HasValue ? DateTime.UtcNow.Add(lockoutDuration.Value) : null;
        AddDomainEvent(new AccountLockedEvent(Id, DateTime.UtcNow, LockoutEnd));
    }

    public void UnlockAccount()
    {
        if (IsActive)
            throw new AccountAlreadyActiveException(Id);

        IsActive = true;
        LockoutEnd = null;
        FailedLoginAttempts = 0;
        AddDomainEvent(new AccountUnlockedEvent(Id, DateTime.UtcNow));
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        AddDomainEvent(new UserActivatedEvent(Id));
    }

    public void UpdateLastLogin(string? ipAddress, string? userAgent)
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();

        // ایجاد رویداد ورود
        AddDomainEvent(new UserLoggedInEvent(Id, ipAddress, userAgent));

        // اضافه کردن به تاریخچه ورود
        LoginHistories.Add(LoginHistory.CreateSuccess(Id, ipAddress, userAgent));
    }

    public void UpdateProfileImage(string url)
    {
        SetValueIfChanged(ref _profileImageUrl, url, () =>
        {
            AddDomainEvent(new ProfileImageChangedEvent(Id, url));
        });
    }

    public void UpdateGender(Gender gender)
    {
        SetValueIfChanged(ref _gender, gender, () =>
        {
            AddDomainEvent(new GenderChangedEvent(Id, gender));
        });
    }

    public void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
        MarkAsUpdated();

        if (FailedLoginAttempts >= 5)
        {
            LockAccount(TimeSpan.FromMinutes(15));
        }
    }

    public void ResetFailedLoginAttempts()
    {
        if (FailedLoginAttempts > 0)
        {
            FailedLoginAttempts = 0;
            MarkAsUpdated();
        }
    }

    public bool IsLockedOut() => LockoutEnd.HasValue && DateTime.UtcNow < LockoutEnd;
    #endregion

    #region Helper Methods
    private static string EnsureValidString(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"مقدار {paramName} نمی‌تواند خالی باشد", paramName);
        return value.Trim();
    }

    /// <summary>
    /// تنظیم مقدار یک فیلد در صورت تغییر
    /// این متد برای جلوگیری از تکرار کد در متدهای Update استفاده می‌شود
    /// </summary>
    /// <typeparam name="T">نوع داده</typeparam>
    /// <param name="field">مراجعه به فیلد</param>
    /// <param name="newValue">مقدار جدید</param>
    /// <param name="onChanged">عملیاتی که در صورت تغییر انجام می‌شود</param>
    private void SetValueIfChanged<T>(ref T field, T newValue, Action onChanged)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue)) return;
        field = newValue;
        onChanged();
        MarkAsUpdated();
    }
    #endregion
}