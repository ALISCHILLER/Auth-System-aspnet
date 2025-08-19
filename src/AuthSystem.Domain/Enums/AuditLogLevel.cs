namespace AuthSystem.Domain.Enums;

/// <summary>
/// سطوح اهمیت برای لاگ‌های حسابرسی
/// این enum برای طبقه‌بندی و فیلتر کردن لاگ‌ها استفاده می‌شود
/// </summary>
public enum AuditLogLevel
{
    /// <summary>
    /// سطح اطلاعات عمومی (مثلاً ورود کاربر)
    /// </summary>
    Information = 1,

    /// <summary>
    /// سطح هشدار (مثلاً تلاش‌های متعدد ورود ناموفق)
    /// </summary>
    Warning = 2,

    /// <summary>
    /// سطح خطا (مثلاً دسترسی غیرمجاز)
    /// </summary>
    Error = 3,

    /// <summary>
    /// سطح خطا بحرانی (مثلاً تلاش برای دسترسی به داده‌های حساس)
    /// </summary>
    Critical = 4,

    /// <summary>
    /// سطح عملیات (مثلاً تغییرات مهم در سیستم)
    /// </summary>
    Operation = 5,

    /// <summary>
    /// سطح امنیتی (مثلاً تغییرات در تنظیمات امنیتی)
    /// </summary>
    Security = 6
}