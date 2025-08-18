// File: AuthSystem.Domain/Enums/HashAlgorithm.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// الگوریتم‌های هش مختلف
/// - این enum برای تعیین الگوریتم هش استفاده می‌شود
/// </summary>
public enum HashAlgorithm
{
    /// <summary>
    /// الگوریتم BCrypt (توصیه شده برای رمز عبور)
    /// </summary>
    BCrypt = 1,

    /// <summary>
    /// الگوریتم Argon2 (امن‌ترین الگوریتم فعلی)
    /// </summary>
    Argon2 = 2,

    /// <summary>
    /// الگوریتم PBKDF2 (قدیمی‌تر اما همچنان امن)
    /// </summary>
    PBKDF2 = 3,

    /// <summary>
    /// الگوریتم SHA256 (برای موارد خاص)
    /// </summary>
    SHA256 = 4,

    /// <summary>
    /// الگوریتم SHA512 (برای موارد خاص)
    /// </summary>
    SHA512 = 5,

    /// <summary>
    /// الگوریتم MD5 (توصیه نمی‌شود - فقط برای سازگاری با سیستم‌های قدیمی)
    /// </summary>
    MD5 = 6
}