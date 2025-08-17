using System;
using System.ComponentModel;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// وضعیت‌های مختلف حساب کاربری در سیستم
/// این enum وضعیت چرخه حیات کاربر را مدیریت می‌کند
/// </summary>
[Flags]
public enum UserStatus
{
    /// <summary>
    /// کاربر در حال ثبت‌نام است (وضعیت موقت)
    /// کاربر ثبت‌نام کرده اما هنوز تأیید نشده است
    /// </summary>
    [Description("در انتظار تأیید")]
    Pending = 1,

    /// <summary>
    /// حساب کاربری فعال است
    /// کاربر می‌تواند به سیستم وارد شود و از امکانات استفاده کند
    /// </summary>
    [Description("فعال")]
    Active = 2,

    /// <summary>
    /// حساب کاربری غیرفعال شده است
    /// کاربر به صورت موقت نمی‌تواند وارد سیستم شود
    /// </summary>
    [Description("غیرفعال")]
    Inactive = 4,

    /// <summary>
    /// حساب کاربری مسدود شده است
    /// معمولاً به دلیل نقض قوانین یا تلاش‌های مشکوک
    /// </summary>
    [Description("مسدود")]
    Blocked = 8,

    /// <summary>
    /// حساب کاربری به حالت تعلیق درآمده است
    /// ممکن است به دلیل عدم فعالیت طولانی یا درخواست کاربر باشد
    /// </summary>
    [Description("معلق")]
    Suspended = 16,

    /// <summary>
    /// حساب کاربری حذف شده است (Soft Delete)
    /// داده‌ها حفظ می‌شوند اما کاربر دسترسی ندارد
    /// </summary>
    [Description("حذف شده")]
    Deleted = 32,

    /// <summary>
    /// حساب کاربری در حال بررسی است
    /// معمولاً برای کاربران با دسترسی‌های خاص که نیاز به تأیید دارند
    /// </summary>
    [Description("در حال بررسی")]
    UnderReview = 64,

    /// <summary>
    /// حساب کاربری منقضی شده است
    /// مثلاً برای حساب‌های موقت یا با تاریخ انقضا
    /// </summary>
    [Description("منقضی شده")]
    Expired = 128,

    /// <summary>
    /// حساب کاربری قفل شده است
    /// معمولاً به دلیل تلاش‌های ناموفق متعدد برای ورود
    /// </summary>
    [Description("قفل شده")]
    Locked = 256,

    /// <summary>
    /// کاربر نیاز به تأیید ایمیل دارد
    /// ایمیل ارسال شده اما هنوز تأیید نشده است
    /// </summary>
    [Description("در انتظار تأیید ایمیل")]
    EmailVerificationPending = 512,

    /// <summary>
    /// کاربر نیاز به تأیید شماره موبایل دارد
    /// پیامک ارسال شده اما هنوز تأیید نشده است
    /// </summary>
    [Description("در انتظار تأیید موبایل")]
    PhoneVerificationPending = 1024,

    /// <summary>
    /// کاربر باید رمز عبور خود را تغییر دهد
    /// معمولاً برای اولین ورود یا بعد از بازنشانی رمز
    /// </summary>
    [Description("نیاز به تغییر رمز عبور")]
    PasswordChangeRequired = 2048,

    /// <summary>
    /// حساب کاربری ترکیب شده است با حساب دیگر
    /// در صورت ادغام حساب‌های کاربری
    /// </summary>
    [Description("ادغام شده")]
    Merged = 4096
}

/// <summary>
/// متدهای کمکی برای UserStatus
/// </summary>
public static class UserStatusExtensions
{
    /// <summary>
    /// بررسی اینکه آیا کاربر می‌تواند وارد سیستم شود
    /// </summary>
    public static bool CanLogin(this UserStatus status)
    {
        return status == UserStatus.Active ||
               status == UserStatus.PasswordChangeRequired;
    }

    /// <summary>
    /// بررسی اینکه آیا وضعیت کاربر نیاز به اقدام دارد
    /// </summary>
    public static bool RequiresAction(this UserStatus status)
    {
        return status.HasFlag(UserStatus.EmailVerificationPending) ||
               status.HasFlag(UserStatus.PhoneVerificationPending) ||
               status.HasFlag(UserStatus.PasswordChangeRequired) ||
               status == UserStatus.Pending ||
               status == UserStatus.UnderReview;
    }

    /// <summary>
    /// بررسی اینکه آیا حساب کاربری قابل بازیابی است
    /// </summary>
    public static bool IsRecoverable(this UserStatus status)
    {
        return status == UserStatus.Inactive ||
               status == UserStatus.Suspended ||
               status == UserStatus.Locked ||
               status == UserStatus.Expired;
    }

    /// <summary>
    /// بررسی اینکه آیا حساب نهایی شده است (نمی‌توان تغییر داد)
    /// </summary>
    public static bool IsFinal(this UserStatus status)
    {
        return status == UserStatus.Deleted ||
               status == UserStatus.Merged;
    }

    /// <summary>
    /// دریافت توضیح فارسی وضعیت
    /// </summary>
    public static string GetDescription(this UserStatus status)
    {
        var type = typeof(UserStatus);
        var memberInfo = type.GetMember(status.ToString());
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        return status.ToString();
    }

    /// <summary>
    /// تبدیل وضعیت به کد برای نمایش در API
    /// </summary>
    public static string ToCode(this UserStatus status)
    {
        return status switch
        {
            UserStatus.Pending => "PND",
            UserStatus.Active => "ACT",
            UserStatus.Inactive => "INA",
            UserStatus.Blocked => "BLK",
            UserStatus.Suspended => "SUS",
            UserStatus.Deleted => "DEL",
            UserStatus.UnderReview => "REV",
            UserStatus.Expired => "EXP",
            UserStatus.Locked => "LCK",
            UserStatus.EmailVerificationPending => "EVP",
            UserStatus.PhoneVerificationPending => "PVP",
            UserStatus.PasswordChangeRequired => "PCR",
            UserStatus.Merged => "MRG",
            _ => "UNK"
        };
    }
}
