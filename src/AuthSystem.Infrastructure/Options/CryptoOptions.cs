using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Infrastructure.Options;

/// <summary>
/// تنظیمات مربوط به رمزنگاری و امنیت
/// </summary>
public class CryptoOptions
{
    public const string SectionName = "Crypto";

    /// <summary>
    /// کلید رمزنگاری (باید 32 بایت باشد و به صورت Base64)
    /// </summary>
    [Required(ErrorMessage = "Crypto:EncryptionKey الزامی است.")]
    [StringLength(44, MinimumLength = 44, ErrorMessage = "Crypto:EncryptionKey باید دقیقاً 32 بایت (44 کاراکتر Base64) باشد.")]
    public string EncryptionKey { get; set; } = string.Empty;

    /// <summary>
    /// کلید JWT
    /// </summary>
    [Required(ErrorMessage = "Crypto:JwtKey الزامی است.")]
    public string JwtKey { get; set; } = string.Empty;

    /// <summary>
    /// صادرکننده توکن JWT
    /// </summary>
    [Required(ErrorMessage = "Crypto:JwtIssuer الزامی است.")]
    public string JwtIssuer { get; set; } = string.Empty;

    /// <summary>
    /// مقصد توکن JWT
    /// </summary>
    [Required(ErrorMessage = "Crypto:JwtAudience الزامی است.")]
    public string JwtAudience { get; set; } = string.Empty;

    /// <summary>
    /// مدت زمان انقضای توکن JWT
    /// </summary>
    public TimeSpan JwtExpiration { get; set; } = TimeSpan.FromHours(2);
}