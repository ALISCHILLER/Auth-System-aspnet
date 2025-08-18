// File: AuthSystem.Domain/Enums/UserStatus.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// وضعیت‌های مختلف کاربر در سیستم
/// - این enum برای مدیریت وضعیت حساب کاربری استفاده می‌شود
/// - هر وضعیت با یک مقدار عددی مشخص شده است
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// کاربر جدیدی که ثبت‌نام کرده اما ایمیلش را تأیید نکرده است
    /// </summary>
    PendingVerification = 1,

    /// <summary>
    /// کاربر فعال و تأیید شده
    /// </summary>
    Active = 2,

    /// <summary>
    /// کاربر غیرفعال (به‌دلیل عدم فعالیت یا اقدامات امنیتی)
    /// </summary>
    Inactive = 3,

    /// <summary>
    /// کاربر قفل شده به‌دلیل تلاش‌های متعدد ناموفق
    /// </summary>
    Locked = 4,

    /// <summary>
    /// کاربر حذف شده (حذف نرم)
    /// </summary>
    Deleted = 5,

    /// <summary>
    /// کاربر موقت (برای مواردی مانند بازیابی رمز عبور)
    /// </summary>
    Temporary = 6,

    /// <summary>
    /// کاربر مسدود شده به‌دلیل نقض قوانین
    /// </summary>
    Banned = 7
}