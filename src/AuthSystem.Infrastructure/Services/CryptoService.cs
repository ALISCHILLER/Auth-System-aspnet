using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
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
/// سرویس مربوط به رمزنگاری، هش کردن و تولید توکن
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
    /// <param name="input">رشته ورودی</param>
    /// <returns>هش SHA256</returns>
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
    /// <param name="plainText">متن معمولی</param>
    /// <returns>متن رمزنگاری شده</returns>
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
    /// <param name="encryptedText">متن رمزنگاری شده</param>
    /// <returns>متن رمزگشایی شده</returns>
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
    /// تولید توکن JWT با اطلاعات کاربر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="email">ایمیل کاربر</param>
    /// <param name="roles">نقش‌های کاربر</param>
    /// <returns>توکن JWT</returns>
    public string GenerateJwtToken(string userId, string email, List<string> roles)
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
            throw new InfrastructureException("خطا در تولید توکن JWT.", ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("خطای ناشناخته در تولید توکن JWT.", ex);
        }
    }
}