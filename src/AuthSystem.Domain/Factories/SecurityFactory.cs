using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Domain.Factories;

/// <summary>
/// Factory برای ایجاد اشیاء امنیتی مستقل از زیرساخت
/// </summary>
public static class SecurityFactory
{

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

        var roleArray = roles?.Distinct().ToArray() ?? Array.Empty<string>();
        if (roleArray.Length == 0)
        {
            throw new ArgumentException("کاربر باید حداقل یک نقش داشته باشد", nameof(roles));
        }

        return TokenValue.Generate(TokenType.Access, 64, expiresIn ?? TimeSpan.FromHours(1));
    }

    public static string CreateRefreshToken(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        return TokenValue.Generate(TokenType.Refresh, 64).Value;
    }


    public static TwoFactorSecretKey CreateTwoFactorSecretKey(string issuer = "AuthSystem")
    {
        if (string.IsNullOrWhiteSpace(issuer))
            throw new ArgumentException("نام صادرکننده نمی‌تواند خالی باشد", nameof(issuer));

        return TwoFactorSecretKey.Generate(issuer);
    }

    public static TwoFactorSecretKey ActivateTwoFactorSecretKey(TwoFactorSecretKey secretKey)
    {
        return secretKey?.Activate() ?? throw new ArgumentNullException(nameof(secretKey));
    }


    public static TwoFactorSecretKey DeactivateTwoFactorSecretKey(TwoFactorSecretKey secretKey)
    {
        return secretKey?.Deactivate() ?? throw new ArgumentNullException(nameof(secretKey));
    }


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

        return VerificationCode.GenerateNumeric(
            VerificationCodeType.EmailVerification,
            length,
            validityMinutes);
    }

    public static VerificationCode CreatePhoneVerificationCode(
        string phoneNumber,
        int length = 6,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("شماره تلفن نمی‌تواند خالی باشد", nameof(phoneNumber));


        if (!PhoneNumber.IsValidPhoneNumber(phoneNumber))
            throw new InvalidPhoneNumberException(phoneNumber, "فرمت شماره تلفن نامعتبر است");

        return VerificationCode.GenerateNumeric(
            VerificationCodeType.PhoneVerification,
            length,
            validityMinutes);
    }


    public static VerificationCode CreateTwoFactorVerificationCode(
        string userId,
        int length = 6,
        int? validityMinutes = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        return VerificationCode.GenerateNumeric(
            VerificationCodeType.TwoFactorAuth,
            length,
            validityMinutes);
    }


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

        return VerificationCode.GenerateAlphanumeric(
            VerificationCodeType.PasswordReset,
            length,
            validityMinutes);
    }


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

        return VerificationCode.GenerateAlphanumeric(
            VerificationCodeType.AccountActivation,
            length,
            validityMinutes);
    }


    public static PasswordHash CreateSecurePassword(string plainPassword)
    {

        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");


        CheckPasswordRules(plainPassword);

        return PasswordHash.CreateFromPlainText(plainPassword);
    }

    public static PasswordHash CreateTemporaryPassword(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("طول رمز عبور موقت باید حداقل 8 کاراکتر باشد", nameof(length));

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var builder = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            builder.Append(chars[bytes[i] % chars.Length]);
        }

        return CreateSecurePassword(builder.ToString());
    }

    public static TokenValue CreateApiToken(
        Guid userId,
        string applicationName,
        TimeSpan? expiresIn = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (string.IsNullOrWhiteSpace(applicationName))
            throw new ArgumentException("نام برنامه نمی‌تواند خالی باشد", nameof(applicationName));

        return TokenValue.Generate(TokenType.ApiKey, 64, expiresIn ?? TimeSpan.FromDays(30));
    }


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

        _ = deviceType; // برای حفظ وابستگی دامنه
        _ = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
        _ = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));

        return TokenValue.Generate(TokenType.Session, 64, TimeSpan.FromHours(2));
    }


    public static string CreateEncryptionKey(int length = 32)
    {
        return GenerateRandomBase64(length);
    }


    public static string CreateEncryptionIv(int length = 16)
    {
        return GenerateRandomBase64(length);
    }
    private static void CheckPasswordRules(string password)
    {
        if (password.Length < 8)
        {
            throw new InvalidPasswordException("رمز عبور باید حداقل 8 کاراکتر باشد");
        }

        if (!password.Any(char.IsUpper))
        {
            throw new InvalidPasswordException("رمز عبور باید حداقل یک حرف بزرگ داشته باشد");
        }

        if (!password.Any(char.IsLower))
        {
            throw new InvalidPasswordException("رمز عبور باید حداقل یک حرف کوچک داشته باشد");
        }

        if (!password.Any(char.IsDigit))
        {
            throw new InvalidPasswordException("رمز عبور باید حداقل یک عدد داشته باشد");
        }

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            throw new InvalidPasswordException("رمز عبور باید حداقل یک کاراکتر خاص داشته باشد");
        }
    }

    private static string GenerateRandomBase64(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

}