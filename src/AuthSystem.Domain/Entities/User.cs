using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Events;
using AuthSystem.Domain.Exceptions;
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
    /// <summary>
    /// نام کاربری منحصر به فرد کاربر
    /// محدودیت طول: حداکثر 50 کاراکتر
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string UserName { get; private set; } = string.Empty;

    /// <summary>
    /// آدرس ایمیل کاربر
    /// این فیلد باید منحصر به فرد باشد و فرمت صحیح داشته باشد
    /// محدودیت طول: حداکثر 255 کاراکتر
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string EmailAddress { get; private set; } = string.Empty;

    /// <summary>
    /// نشان‌دهنده تأیید شدن ایمیل کاربر
    /// مقدار پیش‌فرض: false (ایمیل تأیید نشده)
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// شماره تلفن کاربر
    /// فرمت شماره تلفن باید مطابق با استانداردهای ایران باشد
    /// محدودیت طول: حداکثر 20 کاراکتر
    /// </summary>
    [Phone]
    [MaxLength(20)]
    public string? PhoneNumberValue { get; private set; }

    /// <summary>
    /// نشان‌دهنده تأیید شدن شماره تلفن کاربر
    /// مقدار پیش‌فرض: false (شماره تلفن تأیید نشده)
    /// </summary>
    public bool PhoneNumberConfirmed { get; private set; }

    /// <summary>
    /// کد ملی کاربر
    /// فرمت کد ملی باید مطابق با استانداردهای ایران باشد
    /// </summary>
    [MaxLength(10)]
    public string? NationalCodeValue { get; private set; }

    /// <summary>
    /// هش رمز عبور کاربر
    /// این فیلد نباید هرگز به صورت متن ساده ذخیره شود
    /// </summary>
    [Required]
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// URL تصویر پروفایل کاربر
    /// می‌تواند خالی باشد (در این صورت از تصویر پیش‌فرض استفاده می‌شود)
    /// </summary>
    public string? ProfileImageUrl { get; private set; }

    /// <summary>
    /// وضعیت فعال بودن کاربر
    /// مقدار پیش‌فرض: true (کاربر فعال است)
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// تاریخ و زمان آخرین ورود موفق کاربر
    /// می‌تواند null باشد (اگر کاربر هیچ‌وقت وارد نشده باشد)
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// جنسیت کاربر
    /// مقدار پیش‌فرض: Unknown (نامشخص)
    /// </summary>
    public Gender Gender { get; private set; } = Gender.Unknown;

    /// <summary>
    /// تعداد تلاش‌های ناموفق ورود به سیستم
    /// برای پیاده‌سازی سیستم قفل شدن حساب کاربری استفاده می‌شود
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

    // ویژگی‌های ناوبری (Navigation Properties)

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

    // متدهای دامنه‌ای برای مدیریت وضعیت کاربر

    /// <summary>
    /// ساخت یک کاربر جدید
    /// </summary>
    /// <param name="userName">نام کاربری</param>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="passwordHash">هش رمز عبور</param>
    /// <returns>یک نمونه جدید از کلاس User</returns>
    public static User Create(string userName, string email, string passwordHash,string phoneNumber,string nationalCode,string roleId)
    {
        return new User
        {
            UserName = userName,
            EmailAddress = email,
            PasswordHash = passwordHash,
            PhoneNumberValue=phoneNumber,
            NationalCodeValue=nationalCode,
      
        };
    }

    /// <summary>
    /// به‌روزرسانی ایمیل کاربر
    /// </summary>
    /// <param name="email">آدرس ایمیل جدید</param>
    public void UpdateEmail(string email)
    {
        // اعتبارسنجی ایمیل از طریق ValueObject
        EmailAddress = Email.Create(email).Value;
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
        // اعتبارسنجی شماره تلفن از طریق ValueObject
        PhoneNumberValue = PhoneNumber.Create(phoneNumber).Value;
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
        // اعتبارسنجی کد ملی از طریق ValueObject
        NationalCodeValue = NationalCode.Create(nationalCode).Value;
        AddDomainEvent(new NationalCodeChangedEvent(Id, nationalCode));
    }

    /// <summary>
    /// به‌روزرسانی رمز عبور کاربر
    /// </summary>
    /// <param name="passwordHash">هش رمز عبور جدید</param>
    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        AddDomainEvent(new PasswordChangedEvent(Id));
    }

    /// <summary>
    /// قفل کردن حساب کاربری
    /// </summary>
    /// <param name="lockoutDuration">مدت زمان قفل شدن حساب (در صورت نیاز)</param>
    public void LockAccount(TimeSpan? lockoutDuration = null)
    {
        IsActive = false;

        if (lockoutDuration.HasValue)
            LockoutEnd = DateTime.UtcNow.Add(lockoutDuration.Value);

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
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();
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

        // بررسی آیا تعداد تلاش‌های ناموفق به حد مجاز رسیده است
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
        if (LockoutEnd.HasValue)
            return DateTime.UtcNow < LockoutEnd;

        return false;
    }
}