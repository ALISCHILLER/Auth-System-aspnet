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
/// موجودیت اصلی کاربر در سیستم احراز هویت
/// این کلاس تمام ویژگی‌ها و رفتارهای دامنه‌ای کاربر را تعریف می‌کند
/// </summary>
public class User : BaseEntity
{
    #region Fields (پشتیبان ویژگی‌های خواندنی)
    private string _userName;
    private Email _emailAddress;
    private string _passwordHash;
    private PhoneNumber _phoneNumber;
    private NationalCode _nationalCode;
    #endregion

    #region Properties (فقط خواندنی یا از طریق متد به‌روزرسانی)
    /// <summary>
    /// نام کاربری منحصر به فرد کاربر
    /// محدودیت طول: حداکثر 50 کاراکتر
    /// </summary>
    public string UserName => _userName;

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    public string EmailAddress => _emailAddress.Value;

    /// <summary>
    /// نشان‌دهنده تأیید شدن ایمیل کاربر
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// شماره تلفن کاربر
    /// </summary>
    public string PhoneNumberValue => _phoneNumber.Value;

    /// <summary>
    /// نشان‌دهنده تأیید شدن شماره تلفن کاربر
    /// </summary>
    public bool PhoneNumberConfirmed { get; private set; }

    /// <summary>
    /// کد ملی کاربر
    /// </summary>
    public string NationalCodeValue => _nationalCode.Value;

    /// <summary>
    /// هش رمز عبور کاربر
    /// </summary>
    public string PasswordHash => _passwordHash;

    /// <summary>
    /// URL تصویر پروفایل کاربر
    /// </summary>
    public string? ProfileImageUrl { get; private set; }

    /// <summary>
    /// وضعیت فعال بودن کاربر
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// تاریخ و زمان آخرین ورود موفق کاربر
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    public Gender Gender { get; private set; } = Gender.Unknown;

    /// <summary>
    /// تعداد تلاش‌های ناموفق ورود به سیستم
    /// </summary>
    public int FailedLoginAttempts { get; private set; }

    /// <summary>
    /// زمان پایان قفل حساب کاربری (در صورت قفل شدن)
    /// </summary>
    public DateTime? LockoutEnd { get; private set; }

    /// <summary>
    /// تاریخ انقضای رمز عبور (در صورت استفاده از سیاست انقضای رمز عبور)
    /// </summary>
    public DateTime? PasswordExpiresAt { get; private set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// لیست روابط کاربر با نقش‌ها (برای رابطه چند به چند)
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// لیست توکن‌های تازه‌سازی مربوط به این کاربر
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    /// <summary>
    /// تاریخچه ورودهای کاربر به سیستم
    /// </summary>
    public ICollection<LoginHistory> LoginHistories { get; private set; } = new List<LoginHistory>();

    /// <summary>
    /// لیست دستگاه‌هایی که کاربر از آن‌ها وارد شده است
    /// </summary>
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
    /// <param name="userName">نام کاربری</param>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="passwordHash">هش رمز عبور</param>
    /// <param name="phoneNumber">شماره تلفن</param>
    /// <param name="nationalCode">کد ملی</param>
    /// <returns>یک نمونه جدید از کلاس User</returns>
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
    /// <summary>
    /// به‌روزرسانی ایمیل کاربر
    /// </summary>
    /// <param name="email">آدرس ایمیل جدید</param>
    public void UpdateEmail(string email)
    {
        var newEmail = Email.Create(email);
        if (newEmail == _emailAddress) return;

        _emailAddress = newEmail;
        EmailConfirmed = false;
        AddDomainEvent(new EmailChangedEvent(Id, email));
    }

    /// <summary>
    /// تأیید ایمیل کاربر
    /// </summary>
    public void ConfirmEmail()
    {
        if (EmailConfirmed)
            throw new EmailAlreadyConfirmedException(Id);
        EmailConfirmed = true;
        AddDomainEvent(new EmailConfirmedEvent(Id));
    }

    /// <summary>
    /// به‌روزرسانی شماره تلفن کاربر
    /// </summary>
    /// <param name="phoneNumber">شماره تلفن جدید</param>
    public void UpdatePhoneNumber(string phoneNumber)
    {
        var newPhone = PhoneNumber.Create(phoneNumber);
        if (newPhone == _phoneNumber) return;

        _phoneNumber = newPhone;
        PhoneNumberConfirmed = false;
        AddDomainEvent(new PhoneNumberChangedEvent(Id, phoneNumber));
    }

    /// <summary>
    /// تأیید شماره تلفن کاربر
    /// </summary>
    public void ConfirmPhoneNumber()
    {
        if (PhoneNumberConfirmed)
            throw new PhoneNumberAlreadyConfirmedException(Id);
        PhoneNumberConfirmed = true;
        AddDomainEvent(new PhoneNumberConfirmedEvent(Id));
    }

    /// <summary>
    /// به‌روزرسانی کد ملی کاربر
    /// </summary>
    /// <param name="nationalCode">کد ملی جدید</param>
    public void UpdateNationalCode(string nationalCode)
    {
        var newCode = NationalCode.Create(nationalCode);
        if (newCode == _nationalCode) return;

        _nationalCode = newCode;
        AddDomainEvent(new NationalCodeChangedEvent(Id, nationalCode));
    }

    /// <summary>
    /// به‌روزرسانی رمز عبور کاربر
    /// </summary>
    /// <param name="passwordHash">هش رمز عبور جدید</param>
    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new InvalidPasswordException("هش رمز عبور نمی‌تواند خالی باشد");

        _passwordHash = passwordHash;
        AddDomainEvent(new PasswordChangedEvent(Id));
    }

    /// <summary>
    /// قفل کردن حساب کاربری
    /// </summary>
    /// <param name="lockoutDuration">مدت زمان قفل شدن حساب</param>
    public void LockAccount(TimeSpan? lockoutDuration = null)
    {
        if (!IsActive) return; // اگر قبلاً قفل شده، دوباره قفل نکن

        IsActive = false;
        LockoutEnd = lockoutDuration.HasValue ? DateTime.UtcNow.Add(lockoutDuration.Value) : null;
        AddDomainEvent(new AccountLockedEvent(Id, DateTime.UtcNow, LockoutEnd));
    }

    /// <summary>
    /// باز کردن حساب کاربری قفل شده
    /// </summary>
    public void UnlockAccount()
    {
        if (IsActive)
            throw new AccountAlreadyActiveException(Id);

        IsActive = true;
        LockoutEnd = null;
        FailedLoginAttempts = 0;
        AddDomainEvent(new AccountUnlockedEvent(Id, DateTime.UtcNow));
    }

    /// <summary>
    /// به‌روزرسانی آخرین زمان ورود
    /// </summary>
    /// <param name="ipAddress">آدرس IP ورود</param>
    /// <param name="userAgent">اطلاعات User Agent</param>
    public void UpdateLastLogin(string? ipAddress, string? userAgent)
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();

        // ثبت رویداد ورود
        AddDomainEvent(new UserLoggedInEvent(Id, ipAddress, userAgent));
    }

    /// <summary>
    /// به‌روزرسانی تصویر پروفایل
    /// </summary>
    /// <param name="url">آدرس جدید تصویر پروفایل</param>
    public void UpdateProfileImage(string url)
    {
        ProfileImageUrl = url;
        MarkAsUpdated();
    }

    /// <summary>
    /// به‌روزرسانی جنسیت کاربر
    /// </summary>
    /// <param name="gender">جنسیت جدید</param>
    public void UpdateGender(Gender gender)
    {
        Gender = gender;
        MarkAsUpdated();
    }

    /// <summary>
    /// افزودن یک تلاش ناموفق ورود
    /// </summary>
    public void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
        MarkAsUpdated();

        if (FailedLoginAttempts >= 5)
        {
            LockAccount(TimeSpan.FromMinutes(15));
        }
    }

    /// <summary>
    /// ریست کردن تعداد تلاش‌های ناموفق ورود
    /// </summary>
    public void ResetFailedLoginAttempts()
    {
        if (FailedLoginAttempts > 0)
        {
            FailedLoginAttempts = 0;
            MarkAsUpdated();
        }
    }

    /// <summary>
    /// بررسی آیا حساب کاربری قفل شده است یا خیر
    /// </summary>
    public bool IsLockedOut()
    {
        return LockoutEnd.HasValue && DateTime.UtcNow < LockoutEnd;
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// بررسی و بازگرداندن رشته معتبر (غیر خالی و Trim شده)
    /// </summary>
    /// <param name="value">رشته ورودی</param>
    /// <param name="paramName">نام پارامتر برای پیام خطا</param>
    /// <returns>رشته Trim شده</returns>
    private static string EnsureValidString(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"مقدار {paramName} نمی‌تواند خالی باشد", paramName);
        return value.Trim();
    }
    #endregion
}