namespace AuthSystem.Domain.Enums;

/// <summary>
/// الگوریتم‌های هش پشتیبانی شده
/// این enum برای انتخاب الگوریتم هش در ذخیره‌سازی رمز عبور استفاده می‌شود
/// </summary>
public enum HashAlgorithm
{
    /// <summary>
    /// BCrypt - الگوریتم ایمن و پیشنهادی برای رمز عبور
    /// </summary>
    BCrypt = 1,

    /// <summary>
    /// Argon2 - الگوریتم مدرن و ایمن برای هش کردن
    /// </summary>
    Argon2 = 2,

    /// <summary>
    /// PBKDF2 - الگوریتم استاندارد برای استخراج کلید
    /// </summary>
    PBKDF2 = 3,

    /// <summary>
    /// SHA256 - برای مواردی که نیاز به سرعت بیشتر است
    /// </summary>
    SHA256 = 4,

    /// <summary>
    /// SHA512 - نسخه قوی‌تر SHA256
    /// </summary>
    SHA512 = 5,

    /// <summary>
    /// MD5 - فقط برای سازگاری با سیستم‌های قدیمی (توصیه نمی‌شود)
    /// </summary>
    MD5 = 6
}