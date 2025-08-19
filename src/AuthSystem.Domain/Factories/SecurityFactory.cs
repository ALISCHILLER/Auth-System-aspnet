using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Services.Contracts;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Common.Policies;
using AuthSystem.Domain.Common.Mocks;
using Microsoft.IdentityModel.Tokens;

namespace AuthSystem.Domain.Factories;

/// <summary>
/// Factory برای ایجاد اشیاء امنیتی
/// این کلاس مسئول ایجاد اشیاء مرتبط با امنیت مانند توکن‌ها، کلیدهای 2FA و کدهای تایید است
/// </summary>
public static class SecurityFactory
{
    private static readonly ITokenGenerator _tokenGenerator;
    private static readonly ICryptoProvider _cryptoProvider;

    /// <summary>
    /// سازنده استاتیک با تزریق وابستگی‌ها
    /// </summary>
    static SecurityFactory()
    {
        // در عمل این وابستگی‌ها باید از طریق DI Container تزریق شوند
        // اینجا برای مثال ساده، از نمونه‌های پیش‌فرض استفاده می‌کنیم
        _tokenGenerator = new MockTokenGenerator();
        _cryptoProvider = new MockCryptoProvider();
    }

    /// <summary>
    /// ایجاد توکن دسترسی جدید
    /// </summary>
    public static TokenValue CreateAccessToken(
        Guid userId,
        string username,
        IEnumerable<string> roles,
        TimeSpan? expiresIn = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("نام کاربری نمی‌تواند خالی باشد", nameof(username));

        if (roles == null || !roles.Any())
            throw new ArgumentException("کاربر باید حداقل یک نقش داشته باشد", nameof(roles));

        // تولید توکن با استفاده از سرویس تولید توکن
        var token = _tokenGenerator.GenerateAccessToken(
            userId.ToString(),
            roles.ToArray(),
            expiresIn ?? TimeSpan.FromHours(1));

        return TokenValue.Create(token.Value, TokenType.Access, token.ExpiresAt);
    }

    /// <summary>
    /// ایجاد توکن نوسازی جدید
    /// </summary>
    public static string CreateRefreshToken(Guid userId, string username)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("نام کاربری نمی‌تواند خالی باشد", nameof(username));

        // تولید توکن با استفاده از سرویس تولید توکن
        return _tokenGenerator.GenerateRefreshToken(userId.ToString());
    }

    /// <summary>
    /// ایجاد کلید محرمانه احراز هویت دو عاملی
    /// </summary>
    public static TwoFactorSecretKey CreateTwoFactorSecretKey(string issuer = "AuthSystem")
    {
        if (string.IsNullOrWhiteSpace(issuer))
            throw new ArgumentException("نام صادرکننده نمی‌تواند خالی باشد", nameof(issuer));

        // تولید کلید محرمانه
        var secretKey = TwoFactorSecretKey.Generate(issuer);

        return secretKey;
    }

    /// <summary>
    /// فعال‌سازی کلید محرمانه احراز هویت دو عاملی
    /// </summary>
    public static TwoFactorSecretKey ActivateTwoFactorSecretKey(TwoFactorSecretKey secretKey)
    {
        if (secretKey == null)
            throw new ArgumentNullException(nameof(secretKey));

        // فعال‌سازی کلید
        return secretKey.Activate();
    }

    /// <summary>
    /// غیرفعال‌سازی کلید محرمانه احراز هویت دو عاملی
    /// </summary>
    public static TwoFactorSecretKey DeactivateTwoFactorSecretKey(TwoFactorSecretKey secretKey)
    {
        if (secretKey == null)
            throw new ArgumentNullException(nameof(secretKey));

        // غیرفعال‌سازی کلید
        return secretKey.Deactivate();
    }

    /// <summary>
    /// ایجاد کد تایید جدید برای ایمیل
    /// </summary>
    public static VerificationCode CreateEmailVerificationCode(
        string email,
        int length = 6,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("آدرس ایمیل نمی‌تواند خالی باشد", nameof(email));

        // اعتبارسنجی ایمیل
        if (!Email.IsValidEmail(email))
            throw new InvalidEmailException(email, "فرمت آدرس ایمیل نامعتبر است");

        // تولید کد تایید عددی
        return VerificationCode.GenerateNumeric(
            VerificationCodeType.EmailVerification,
            length,
            validityMinutes);
    }

    /// <summary>
    /// ایجاد کد تایید جدید برای شماره تلفن
    /// </summary>
    public static VerificationCode CreatePhoneVerificationCode(
        string phoneNumber,
        int length = 6,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("شماره تلفن نمی‌تواند خالی باشد", nameof(phoneNumber));

        // اعتبارسنجی شماره تلفن
        if (!PhoneNumber.IsValidPhoneNumber(phoneNumber))
            throw new InvalidPhoneNumberException(phoneNumber, "فرمت شماره تلفن نامعتبر است");

        // تولید کد تایید عددی
        return VerificationCode.GenerateNumeric(
            VerificationCodeType.PhoneVerification,
            length,
            validityMinutes);
    }

    /// <summary>
    /// ایجاد کد تایید جدید برای احراز هویت دو عاملی
    /// </summary>
    public static VerificationCode CreateTwoFactorVerificationCode(
        string userId,
        int length = 6,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        // تولید کد تایید عددی
        return VerificationCode.GenerateNumeric(
            VerificationCodeType.TwoFactorAuth,
            length,
            validityMinutes);
    }

    /// <summary>
    /// ایجاد کد تایید جدید برای بازیابی رمز عبور
    /// </summary>
    public static VerificationCode CreatePasswordResetCode(
        string email,
        int length = 8,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("آدرس ایمیل نمی‌تواند خالی باشد", nameof(email));

        // اعتبارسنجی ایمیل
        if (!Email.IsValidEmail(email))
            throw new InvalidEmailException(email, "فرمت آدرس ایمیل نامعتبر است");

        // تولید کد تایید الفبایی-عددی
        return VerificationCode.GenerateAlphanumeric(
            VerificationCodeType.PasswordReset,
            length,
            validityMinutes);
    }

    /// <summary>
    /// ایجاد کد تایید جدید برای فعال‌سازی حساب
    /// </summary>
    public static VerificationCode CreateAccountActivationCode(
        string email,
        int length = 8,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("آدرس ایمیل نمی‌تواند خالی باشد", nameof(email));

        // اعتبارسنجی ایمیل
        if (!Email.IsValidEmail(email))
            throw new InvalidEmailException(email, "فرمت آدرس ایمیل نامعتبر است");

        // تولید کد تایید الفبایی-عددی
        return VerificationCode.GenerateAlphanumeric(
            VerificationCodeType.AccountActivation,
            length,
            validityMinutes);
    }

    /// <summary>
    /// ایجاد رمز عبور امن جدید
    /// </summary>
    public static PasswordHash CreateSecurePassword(string plainPassword)
    {
        // اعتبارسنجی رمز عبور
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");

        // بررسی قوانین امنیتی رمز عبور
        CheckPasswordRules(plainPassword);

        // ایجاد هش رمز عبور
        return PasswordHash.CreateFromPlainText(plainPassword);
    }

    /// <summary>
    /// بررسی قوانین امنیتی رمز عبور
    /// </summary>
    private static void CheckPasswordRules(string password)
    {
        // بررسی طول رمز عبور
        if (password.Length < 8)
            throw new InvalidPasswordException("رمز عبور باید حداقل 8 کاراکتر باشد");

        // بررسی وجود حروف بزرگ
        if (!password.Any(char.IsUpper))
            throw new InvalidPasswordException("رمز عبور باید حداقل یک حرف بزرگ داشته باشد");

        // بررسی وجود حروف کوچک
        if (!password.Any(char.IsLower))
            throw new InvalidPasswordException("رمز عبور باید حداقل یک حرف کوچک داشته باشد");

        // بررسی وجود اعداد
        if (!password.Any(char.IsDigit))
            throw new InvalidPasswordException("رمز عبور باید حداقل یک عدد داشته باشد");

        // بررسی وجود کاراکترهای خاص
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            throw new InvalidPasswordException("رمز عبور باید حداقل یک کاراکتر خاص داشته باشد");
    }

    /// <summary>
    /// ایجاد رمز عبور موقت ایمن
    /// </summary>
    public static PasswordHash CreateTemporaryPassword(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("طول رمز عبور موقت باید حداقل 8 کاراکتر باشد", nameof(length));

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var password = new char[length];
        for (int i = 0; i < length; i++)
        {
            password[i] = chars[bytes[i] % chars.Length];
        }

        return CreateSecurePassword(new string(password));
    }

    /// <summary>
    /// ایجاد توکن API جدید
    /// </summary>
    public static TokenValue CreateApiToken(
        Guid userId,
        string applicationName,
        TimeSpan? expiresIn = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (string.IsNullOrWhiteSpace(applicationName))
            throw new ArgumentException("نام برنامه نمی‌تواند خالی باشد", nameof(applicationName));

        // تولید توکن امن
        var token = _tokenGenerator.GenerateAccessToken(
            $"{userId}-api",
            new[] { "ApiAccess" },
            expiresIn ?? TimeSpan.FromDays(30));

        return TokenValue.Create(token.Value, TokenType.ApiKey, token.ExpiresAt);
    }

    /// <summary>
    /// ایجاد توکن جلسه جدید
    /// </summary>
    public static TokenValue CreateSessionToken(
        Guid userId,
        Guid sessionId,
        DeviceType deviceType,
        UserAgent userAgent,
        IpAddress ipAddress)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (sessionId == Guid.Empty)
            throw new ArgumentException("شناسه جلسه نمی‌تواند خالی باشد", nameof(sessionId));

        // تولید توکن جلسه
        var token = _tokenGenerator.GenerateAccessToken(
            $"{userId}-session-{sessionId}",
            new[] { "Session" },
            TimeSpan.FromHours(2));

        return TokenValue.Create(token.Value, TokenType.Session, token.ExpiresAt);
    }

    /// <summary>
    /// ایجاد کلید رمزنگاری جدید
    /// </summary>
    public static string CreateEncryptionKey(int length = 32)
    {
        return _cryptoProvider.GenerateRandomKey(length);
    }

    /// <summary>
    /// ایجاد IV رمزنگاری جدید
    /// </summary>
    public static string CreateEncryptionIv(int length = 16)
    {
        return _cryptoProvider.GenerateRandomIv(length);
    }
}