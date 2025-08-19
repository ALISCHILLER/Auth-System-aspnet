namespace AuthSystem.Domain.Enums;

/// <summary>
/// وضعیت‌های کاربر در سیستم
/// این enum برای مدیریت وضعیت کاربران استفاده می‌شود
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// کاربر فعال و می‌تواند وارد سیستم شود
    /// </summary>
    Active = 1,

    /// <summary>
    /// کاربر قفل شده است (به دلیل تلاش‌های متعدد ناموفق)
    /// </summary>
    Locked = 2,

    /// <summary>
    /// کاربر در انتظار تأیید ایمیل است
    /// </summary>
    Pending = 3,

    /// <summary>
    /// کاربر حذف شده است (حذف منطقی)
    /// </summary>
    Deleted = 4,

    /// <summary>
    /// کاربر تأیید نشده است
    /// </summary>
    Unverified = 5,

    /// <summary>
    /// کاربر غیرفعال شده توسط ادمین
    /// </summary>
    Inactive = 6,

    /// <summary>
    /// کاربر به دلیل فعالیت‌های مشکوک معلق شده است
    /// </summary>
    Suspended = 7,

    /// <summary>
    /// کاربر به دلیل فعالیت‌های غیرمجاز مسدود شده است
    /// </summary>
    Banned = 8
}