using System.ComponentModel;

namespace AuthSystem.Domain.Enums;


/// <summary>
/// انواع کد تایید در سیستم
/// هر نوع کد تأیید ویژگی‌ها و محدودیت‌های خاص خود را دارد
/// </summary>
public enum VerificationCodeType
{
    /// <summary>
    /// تایید آدرس ایمیل
    /// برای تأیید مالکیت ایمیل در هنگام ثبت‌نام یا تغییر ایمیل
    /// </summary>
    [Description("تأیید ایمیل")]
    EmailVerification = 1,

    /// <summary>
    /// تایید شماره تلفن
    /// برای تأیید مالکیت شماره موبایل
    /// </summary>
    [Description("تأیید موبایل")]
    PhoneVerification = 2,

    /// <summary>
    /// احراز هویت دو عاملی (2FA)
    /// کد موقت برای ورود ایمن‌تر
    /// </summary>
    [Description("احراز هویت دو عاملی")]
    TwoFactorAuth = 3,

    /// <summary>
    /// بازیابی رمز عبور
    /// کد برای بازنشانی رمز عبور فراموش شده
    /// </summary>
    [Description("بازیابی رمز عبور")]
    PasswordReset = 4,

    /// <summary>
    /// فعال‌سازی حساب کاربری
    /// کد برای فعال‌سازی اولیه حساب
    /// </summary>
    [Description("فعال‌سازی حساب")]
    AccountActivation = 5,

    /// <summary>
    /// تایید تراکنش
    /// کد برای تأیید عملیات حساس مالی یا امنیتی
    /// </summary>
    [Description("تأیید تراکنش")]
    Transaction = 6,

    /// <summary>
    /// تغییر ایمیل
    /// کد برای تأیید ایمیل جدید در هنگام تغییر
    /// </summary>
    [Description("تغییر ایمیل")]
    EmailChange = 7,

    /// <summary>
    /// تغییر شماره موبایل
    /// کد برای تأیید شماره جدید در هنگام تغییر
    /// </summary>
    [Description("تغییر موبایل")]
    PhoneChange = 8,

    /// <summary>
    /// ورود از دستگاه جدید
    /// کد برای تأیید ورود از دستگاه ناشناس
    /// </summary>
    [Description("دستگاه جدید")]
    NewDeviceLogin = 9,

    /// <summary>
    /// حذف حساب کاربری
    /// کد برای تأیید درخواست حذف حساب
    /// </summary>
    [Description("حذف حساب")]
    AccountDeletion = 10,

    /// <summary>
    /// دسترسی API
    /// کد برای تأیید ایجاد یا تغییر کلید API
    /// </summary>
    [Description("دسترسی API")]
    APIAccess = 11,

    /// <summary>
    /// تأیید هویت اضطراری
    /// کد برای دسترسی اضطراری در صورت مشکل در روش‌های دیگر
    /// </summary>
    [Description("تأیید اضطراری")]
    EmergencyVerification = 12,

    /// <summary>
    /// لینک ورود یکبار مصرف
    /// کد برای ورود بدون رمز عبور (Magic Link)
    /// </summary>
    [Description("لینک ورود")]
    MagicLink = 13,

    /// <summary>
    /// تأیید عملیات مدیریتی
    /// کد برای تأیید عملیات‌های حساس مدیریتی
    /// </summary>
    [Description("عملیات مدیریتی")]
    AdminOperation = 14
}