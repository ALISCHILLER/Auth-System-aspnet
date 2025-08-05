using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Helpers;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Infrastructure.Services;

/// <summary>
/// پیاده‌سازی کامل ICryptoService با استفاده از بهترین روش‌های امنیتی
/// </summary>
public class CryptoService : ICryptoService
{
    private readonly CryptoOptions _options;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="options">تنظیمات رمزنگاری</param>
    public CryptoService(IOptions<CryptoOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        // اعتبارسنجی تنظیمات
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(_options);

        if (!Validator.TryValidateObject(_options, validationContext, validationResults, true))
        {
            var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidConfigurationException("Crypto", errors);
        }
    }

    /// <summary>
    /// تولید هش SHA256 از یک رشته ورودی
    /// </summary>
    public string GenerateSha256(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("ورودی نمی‌تواند خالی باشد", nameof(input));

        try
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید هش SHA256.", ex);
        }
    }

    /// <summary>
    /// رمزنگاری متقارن با AES و کلید از پیکربندی
    /// </summary>
    public string Encrypt(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("متن معمولی نمی‌تواند خالی باشد", nameof(plainText));

        try
        {
            var keyBytes = Convert.FromBase64String(_options.EncryptionKey);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = Convert.ToBase64String(aes.IV) + "." + Convert.ToBase64String(cipherBytes);
            return result;
        }
        catch (FormatException ex)
        {
            throw new InvalidConfigurationException("Encryption:Key", "کلید رمزنگاری باید به صورت Base64 باشد.");
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در رمزنگاری متن.", ex);
        }
    }

    /// <summary>
    /// رمزگشایی متن رمزنگاری شده با AES
    /// </summary>
    public string Decrypt(string encryptedText)
    {
        if (string.IsNullOrWhiteSpace(encryptedText))
            throw new ArgumentException("متن رمزنگاری شده نمی‌تواند خالی باشد", nameof(encryptedText));

        try
        {
            var parts = encryptedText.Split('.');
            if (parts.Length != 2)
                throw new ArgumentException("فرمت متن رمزنگاری شده نامعتبر است.", nameof(encryptedText));

            var iv = Convert.FromBase64String(parts[0]);
            var cipherBytes = Convert.FromBase64String(parts[1]);

            var keyBytes = Convert.FromBase64String(_options.EncryptionKey);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("کلید رمزنگاری یا متن رمزنگاری شده نامعتبر است.");
        }
        catch (CryptographicException ex)
        {
            throw new ArgumentException("رمزگشایی متن با خطا مواجه شد. ممکن است کلید اشتباه باشد یا داده خراب شده باشد.", ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطای ناشناخته در رمزگشایی متن.", ex);
        }
    }

    /// <summary>
    /// تولید یک توکن دسترسی (Access Token) با استفاده از JWT
    /// </summary>
    public string GenerateAccessToken(
        string userId,
        string email,
        List<string> roles,
        Dictionary<string, object>? additionalClaims = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("ایمیل کاربر نمی‌تواند خالی باشد", nameof(email));

        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_options.JwtKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles ?? new List<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));

            // افزودن کلیم اضافی
            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value.ToString()!));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _options.JwtIssuer,
                audience: _options.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_options.JwtExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (SecurityTokenException ex)
        {
            throw new InfrastructureException("خطا در تولید توکن دسترسی.", ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطای ناشناخته در تولید توکن دسترسی.", ex);
        }
    }

    /// <summary>
    /// تولید یک توکن تازه‌سازی (Refresh Token)
    /// </summary>
    public string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید توکن تازه‌سازی.", ex);
        }
    }

    /// <summary>
    /// تولید همزمان توکن دسترسی و تازه‌سازی
    /// </summary>
    public (string AccessToken, string RefreshToken) GenerateTokens(
        string userId,
        string email,
        List<string> roles,
        Dictionary<string, object>? additionalClaims = null)
    {
        var accessToken = GenerateAccessToken(userId, email, roles, additionalClaims);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken);
    }

    /// <summary>
    /// تولید یک توکن تأیید (Verification Token) برای تأیید ایمیل یا شماره تلفن
    /// </summary>
    public string GenerateVerificationToken(string purpose, string userId)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("هدف تولید توکن نمی‌تواند خالی باشد", nameof(purpose));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_options.JwtKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("purpose", purpose),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _options.JwtIssuer,
                audience: _options.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_options.VerificationTokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید توکن تأیید.", ex);
        }
    }

    /// <summary>
    /// تولید یک کلید یکتا برای کاربر (مثلاً برای 2FA)
    /// </summary>
    public string GenerateUserSecretKey()
    {
        try
        {
            var randomNumber = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                // تبدیل به فرمت Base32 برای سازگاری با Google Authenticator
                return Base32Encoding.ToString(randomNumber);
            }
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید کلید کاربر.", ex);
        }
    }

    /// <summary>
    /// بررسی صحت یک کد یک‌بار مصرف (OTP)
    /// </summary>
    public bool VerifyOtpCode(string secretKey, string code)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            return false;

        if (string.IsNullOrWhiteSpace(code))
            return false;

        try
        {
            var secretKeyBytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(secretKeyBytes, step: 30, totpSize: 6);
            return totp.VerifyCode(code);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// تولید یک کد یک‌بار مصرف (OTP) برای احراز هویت دو مرحله‌ای
    /// </summary>
    public string GenerateOtpCode(string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("کلید محرمانه نمی‌تواند خالی باشد", nameof(secretKey));

        try
        {
            var secretKeyBytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(secretKeyBytes, step: 30, totpSize: 6);
            return totp.ComputeTotp();
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید کد OTP.", ex);
        }
    }

    /// <summary>
    /// تولید یک توکن موقت برای بازیابی رمز عبور
    /// </summary>
    public string GeneratePasswordResetToken(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("شناسه کاربر نمی‌تواند خالی باشد", nameof(userId));

        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_options.JwtKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("purpose", "PasswordReset"),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _options.JwtIssuer,
                audience: _options.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_options.PasswordResetTokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید توکن بازیابی رمز عبور.", ex);
        }
    }

    /// <summary>
    /// تولید یک توکن امن برای ارتباطات داخلی
    /// </summary>
    public string GenerateInternalToken()
    {
        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_options.JwtKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("service", "auth-system"),
                new Claim("internal", "true"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _options.JwtIssuer,
                audience: _options.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطا در تولید توکن داخلی.", ex);
        }
    }
}