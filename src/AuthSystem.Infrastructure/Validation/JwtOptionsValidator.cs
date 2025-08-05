using AuthSystem.Infrastructure.Options;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Infrastructure.Validation;

/// <summary>
/// کلاس کمکی برای اعتبارسنجی JwtOptions
/// </summary>
public static class JwtOptionsValidator
{
    /// <summary>
    /// اعتبارسنجی تنظیمات JWT
    /// </summary>
    /// <param name="options">تنظیمات JWT</param>
    /// <returns>لیست خطاها</returns>
    public static List<ValidationResult> Validate(JwtOptions options)
    {
        var results = new List<ValidationResult>();
        var validationContext = new ValidationContext(options, null, null);

        Validator.TryValidateObject(options, validationContext, results, true);

        // اعتبارسنجی‌های خاص JWT
        ValidateSecretKeyStrength(options, results);
        ValidateTimeSettings(options, results);
        ValidateBoundSettings(options, results);

        return results;
    }

    /// <summary>
    /// اعتبارسنجی قدرت کلید مخفی
    /// </summary>
    private static void ValidateSecretKeyStrength(JwtOptions options, List<ValidationResult> results)
    {
        if (string.IsNullOrWhiteSpace(options.SecretKey))
            return;

        // بررسی طول کلید
        if (options.SecretKey.Length < 32)
        {
            results.Add(new ValidationResult(
                "طول کلید مخفی باید حداقل 32 کاراکتر باشد برای امنیت کافی.",
                new[] { nameof(JwtOptions.SecretKey) }));
        }

        // بررسی پیچیدگی کلید
        bool hasUpper = options.SecretKey.Any(char.IsUpper);
        bool hasLower = options.SecretKey.Any(char.IsLower);
        bool hasDigit = options.SecretKey.Any(char.IsDigit);
        bool hasSpecial = options.SecretKey.Any(c => !char.IsLetterOrDigit(c));

        if (!hasUpper || !hasLower || !hasDigit || !hasSpecial)
        {
            results.Add(new ValidationResult(
                "کلید مخفی باید شامل حروف بزرگ، حروف کوچک، اعداد و کاراکترهای خاص باشد.",
                new[] { nameof(JwtOptions.SecretKey) }));
        }
    }

    /// <summary>
    /// اعتبارسنجی تنظیمات زمانی
    /// </summary>
    private static void ValidateTimeSettings(JwtOptions options, List<ValidationResult> results)
    {
        if (options.AccessTokenExpirationMinutes <= 0)
        {
            results.Add(new ValidationResult(
                "مدت زمان انقضای توکن دسترسی باید بیشتر از صفر باشد.",
                new[] { nameof(JwtOptions.AccessTokenExpirationMinutes) }));
        }

        if (options.RefreshTokenExpirationDays <= 0)
        {
            results.Add(new ValidationResult(
                "مدت زمان انقضای توکن تازه‌سازی باید بیشتر از صفر باشد.",
                new[] { nameof(JwtOptions.RefreshTokenExpirationDays) }));
        }

        if (options.AccessTokenExpirationMinutes >= options.RefreshTokenExpirationDays * 24 * 60)
        {
            results.Add(new ValidationResult(
                "مدت زمان انقضای توکن دسترسی نباید بیشتر از مدت زمان انقضای توکن تازه‌سازی باشد.",
                new[] { nameof(JwtOptions.AccessTokenExpirationMinutes) }));
        }
    }

    /// <summary>
    /// اعتبارسنجی تنظیمات محدودیت
    /// </summary>
    private static void ValidateBoundSettings(JwtOptions options, List<ValidationResult> results)
    {
        if (options.MaxActiveTokensPerUser <= 0)
        {
            results.Add(new ValidationResult(
                "حداکثر تعداد توکن‌های فعال باید بیشتر از صفر باشد.",
                new[] { nameof(JwtOptions.MaxActiveTokensPerUser) }));
        }

        if (options.MaxTokenUsageCount <= 0)
        {
            results.Add(new ValidationResult(
                "حداکثر تعداد استفاده از توکن باید بیشتر از صفر باشد.",
                new[] { nameof(JwtOptions.MaxTokenUsageCount) }));
        }
    }
}