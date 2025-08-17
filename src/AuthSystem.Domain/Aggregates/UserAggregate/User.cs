using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Domain.Aggregates.UserAggregate.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate;

/// <summary>
/// کلاس اصلی Aggregate Root سیستم احراز هویت
/// این کلاس مسئول مدیریت کل چرخه حیات کاربر و اعمال قوانین کسب‌وکار است
/// </summary>
public sealed class User : AggregateRoot<Guid>
{
    // ------------------------
    // ویژگی‌های اصلی کاربر
    // ------------------------

    /// <summary>
    /// نام کاربری (غیرقابل تغییر پس از ایجاد)
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// هش رمز عبور کاربر
    /// </summary>
    public PasswordHash PasswordHash { get; private set; }

    /// <summary>
    /// شماره تلفن کاربر (اختیاری)
    /// </summary>
    public PhoneNumber? PhoneNumber { get; private set; }

    /// <summary>
    /// کد ملی کاربر (اختیاری)
    /// </summary>
    public NationalCode? NationalCode { get; private set; }

    /// <summary>
    /// وضعیت فعلی کاربر
    /// </summary>
    public UserStatus Status { get; private set; }

    /// <summary>
    /// تاریخ ایجاد کاربر (همیشه UTC)
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی (همیشه UTC)
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// تاریخ آخرین ورود موفق به سیستم (اختیاری)
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// تعداد ورودهای ناموفق متوالی
    /// </summary>
    public int FailedLoginAttempts { get; private set; }

    /// <summary>
    /// زمان پایان قفل شدن حساب (در صورت قفل بودن)
    /// </summary>
    public DateTime? LockoutEnd { get; private set; }

    /// <summary>
    /// کلید محرمانه احراز هویت دو عاملی (در صورت فعال بودن)
    /// </summary>
    public TwoFactorSecretKey? TwoFactorSecretKey { get; private set; }

    /// <summary>
    /// توکن تایید ایمیل (در صورت نیاز به تایید)
    /// </summary>
    public TokenValue? EmailVerificationToken { get; private set; }

    // ------------------------
    // مجموعه‌های مرتبط
    // ------------------------

    /// <summary>
    /// تاریخچه ورودهای کاربر
    /// </summary>
    private readonly List<LoginHistory> _loginHistories = new();

    /// <summary>
    /// دستگاه‌های ثبت شده کاربر
    /// </summary>
    private readonly List<UserDevice> _userDevices = new();

    /// <summary>
    /// دسترسی به تاریخچه ورودها (فقط خواندنی)
    /// </summary>
    public IReadOnlyCollection<LoginHistory> LoginHistories => _loginHistories.AsReadOnly();

    /// <summary>
    /// دسترسی به دستگاه‌های کاربر (فقط خواندنی)
    /// </summary>
    public IReadOnlyCollection<UserDevice> UserDevices => _userDevices.AsReadOnly();

    // ------------------------
    // سازنده‌ها
    // ------------------------

    /// <summary>
    /// سازنده پیش‌فرض برای ORM و سریال‌سازی
    /// </summary>
    private User()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// سازنده اصلی برای ایجاد کاربر جدید
    /// </summary>
    private User(
        string username,
        Email email,
        Password password,
        PhoneNumber? phoneNumber = null)
    {
        Id = Guid.NewGuid();
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = PasswordHash.CreateFromPlainText(password.Value);
        PhoneNumber = phoneNumber;
        Status = UserStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        // افزودن رویداد ثبت‌نام
        AddDomainEvent(new UserRegisteredEvent(
            Id,
            email.Value,
            phoneNumber?.Value,
            Guid.NewGuid().ToString()));
    }

    // ------------------------
    // متدهای سازنده
    // ------------------------

    /// <summary>
    /// ایجاد کاربر جدید
    /// </summary>
    public static User Create(
        string username,
        Email email,
        Password password,
        PhoneNumber? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("نام کاربری نمی‌تواند خالی باشد", nameof(username));

        return new User(username, email, password, phoneNumber);
    }

    // ------------------------
    // متدهای کسب‌وکار اصلی
    // ------------------------

    /// <summary>
    /// ورود به سیستم
    /// </summary>
    public LoginResult Login(
        Password password,
        IpAddress ipAddress,
        UserAgent userAgent)
    {
        // بررسی قفل بودن حساب
        if (IsLocked)
        {
            var remaining = LockoutEnd.Value - DateTime.UtcNow;
            return LoginResult.AccountLocked;
        }

        // بررسی رمز عبور
        if (!PasswordHash.Verify(password.Value))
        {
            HandleFailedLogin();
            return LoginResult.InvalidPassword;
        }

        // ورود موفق
        ResetFailedLoginAttempts();
        LastLoginAt = DateTime.UtcNow;

        // ثبت تاریخچه ورود
        var loginHistory = LoginHistory.Create(Id, ipAddress, userAgent, true);
        _loginHistories.Add(loginHistory);

        // افزودن رویداد ورود
        AddDomainEvent(new UserLoggedInEvent(
            Id,
            Email.Value,
            ipAddress.Value,
            Guid.NewGuid().ToString()));

        return Status.RequiresAction() ? LoginResult.AccountNotVerified : LoginResult.Success;
    }

    /// <summary>
    /// مدیریت ورود ناموفق
    /// </summary>
    private void HandleFailedLogin()
    {
        FailedLoginAttempts++;
        UpdatedAt = DateTime.UtcNow;

        if (FailedLoginAttempts >= 5)
        {
            LockoutEnd = DateTime.UtcNow.AddMinutes(15);
            AddDomainEvent(new UserLockedEvent(
                Id,
                "تلاش‌های ناموفق متعدد",
                Guid.NewGuid().ToString()));
        }
    }

    /// <summary>
    /// بازنشانی تعداد ورودهای ناموفق
    /// </summary>
    private void ResetFailedLoginAttempts()
    {
        if (FailedLoginAttempts > 0)
        {
            FailedLoginAttempts = 0;
            LockoutEnd = null;
            AddDomainEvent(new UserUnlockedEvent(
                Id,
                Guid.NewGuid().ToString()));
        }
    }

    /// <summary>
    /// تغییر رمز عبور
    /// </summary>
    public void ChangePassword(Password currentPassword, Password newPassword)
    {
        if (!PasswordHash.Verify(currentPassword))
            throw InvalidPasswordException.ForIncorrectCurrentPassword();

        if (PasswordHash.Verify(newPassword))
            throw new InvalidPasswordException("رمز عبور جدید نمی‌تواند مشابه رمز عبور فعلی باشد");

        PasswordHash = PasswordHash.CreateFromPlainText(newPassword);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(
            Id,
            Email.Value,
            Guid.NewGuid().ToString()));
    }

    /// <summary>
    /// فعال‌سازی حساب کاربری
    /// </summary>
    public void ActivateAccount(string triggeredBy = "System")
    {
        if (Status != UserStatus.Pending)
            throw new DomainException("حساب کاربری قبلاً فعال شده است");

        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserStatusChangedEvent(
            Id,
            UserStatus.Pending,
            UserStatus.Active,
            "فعال‌سازی اولیه",
            triggeredBy));
    }

    /// <summary>
    /// تغییر ایمیل
    /// </summary>
    public void ChangeEmail(Email newEmail, string triggeredBy = "User")
    {
        if (newEmail == Email)
            return;

        Email = newEmail;
        EmailVerificationToken = TokenValue.Generate(TokenType.EmailVerification, validity: TimeSpan.FromHours(24));
        Status = Status.WithFlag(UserStatus.EmailVerificationPending);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserProfileUpdatedEvent(
            Id,
            new[] { "Email" },
            triggeredBy));
    }

    /// <summary>
    /// تأیید ایمیل
    /// </summary>
    public void VerifyEmail(TokenValue token, string triggeredBy = "User")
    {
        if (EmailVerificationToken is null)
            throw new DomainException("هیچ توکن تأیید ایمیلی وجود ندارد");

        if (token.Value != EmailVerificationToken.Value)
            throw new DomainException("توکن تأیید ایمیل نامعتبر است");

        if (token.IsExpired)
            throw new DomainException("توکن تأیید ایمیل منقضی شده است");

        EmailVerificationToken = null;
        Status = Status.RemoveFlag(UserStatus.EmailVerificationPending);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new EmailVerifiedEvent(
            Id,
            Email.Value,
            triggeredBy));
    }

    /// <summary>
    /// فعال‌سازی احراز هویت دو عاملی
    /// </summary>
    public TwoFactorSecretKey EnableTwoFactor(string triggeredBy = "User")
    {
        if (IsTwoFactorEnabled)
            throw new TwoFactorException.ForAlreadyEnabled(Id);

        var secretKey = TwoFactorSecretKey.Generate();
        TwoFactorSecretKey = secretKey;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TwoFactorEnabledEvent(
            Id,
            triggeredBy));

        return secretKey;
    }

    /// <summary>
    /// تأیید و فعال‌سازی نهایی احراز هویت دو عاملی
    /// </summary>
    public void ConfirmTwoFactor(VerificationCode code, string triggeredBy = "User")
    {
        if (!IsTwoFactorEnabled || TwoFactorSecretKey == null)
            throw new TwoFactorException.ForNotEnabled(Id);

        if (!TwoFactorSecretKey.Verify(code.Value))
            throw new InvalidTwoFactorSecretKeyException.ForInvalidCode(code.Value);

        TwoFactorSecretKey = TwoFactorSecretKey.Activate();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// غیرفعال‌سازی احراز هویت دو عاملی
    /// </summary>
    public void DisableTwoFactor(string triggeredBy = "User")
    {
        if (!IsTwoFactorEnabled)
            throw new TwoFactorException.ForNotEnabled(Id);

        TwoFactorSecretKey = TwoFactorSecretKey?.Deactivate();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TwoFactorDisabledEvent(
            Id,
            triggeredBy));
    }

    /// <summary>
    /// درخواست بازیابی رمز عبور
    /// </summary>
    public TokenValue RequestPasswordReset(string triggeredBy = "User")
    {
        var token = TokenValue.Generate(TokenType.PasswordReset, validity: TimeSpan.FromMinutes(15));
        AddDomainEvent(new UserPasswordResetRequestedEvent(
            Id,
            Email.Value,
            token.Value,
            triggeredBy));
        return token;
    }

    /// <summary>
    /// تکمیل بازیابی رمز عبور
    /// </summary>
    public void CompletePasswordReset(TokenValue token, Password newPassword, string triggeredBy = "User")
    {
        if (token.Type != TokenType.PasswordReset)
            throw new InvalidTokenException("نوع توکن نامعتبر است");

        if (token.IsExpired)
            throw new InvalidTokenException("توکن منقضی شده است");

        PasswordHash = PasswordHash.CreateFromPlainText(newPassword.Value);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordResetCompletedEvent(
            Id,
            Email.Value,
            triggeredBy));
    }

    // ------------------------
    // متدهای وضعیت
    // ------------------------

    /// <summary>
    /// آیا کاربر می‌تواند وارد شود
    /// </summary>
    public bool CanLogin => Status.CanLogin() && !IsLocked;

    /// <summary>
    /// آیا کاربر قفل شده است
    /// </summary>
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow;

    /// <summary>
    /// آیا ایمیل تأیید شده است
    /// </summary>
    public bool IsEmailVerified => EmailVerificationToken is null;

    /// <summary>
    /// آیا احراز هویت دو عاملی فعال است
    /// </summary>
    public bool IsTwoFactorEnabled => TwoFactorSecretKey?.IsActive == true;
}