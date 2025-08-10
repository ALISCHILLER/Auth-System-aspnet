using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// وضعیت‌های مختلف احراز هویت در سیستم
/// این enum برای نمایش وضعیت عملیات احراز هویت استفاده می‌شود
/// </summary>
public enum AuthStatus
{
    /// <summary>
    /// عملیات احراز هویت با موفقیت انجام شد
    /// </summary>
    [Display(Name = "موفقیت", Description = "عملیات با موفقیت انجام شد")]
    Success,

    /// <summary>
    /// اعتبارسنجی ناموفق بود (نام کاربری یا رمز عبور اشتباه)
    /// </summary>
    [Display(Name = "اعتبارسنجی ناموفق", Description = "نام کاربری یا رمز عبور اشتباه است")]
    InvalidCredentials,

    /// <summary>
    /// حساب کاربری به دلیل تلاش‌های مکرر ناموفق قفل شده است
    /// </summary>
    [Display(Name = "حساب قفل شده", Description = "حساب کاربری قفل شده است")]
    AccountLocked,

    /// <summary>
    /// ایمیل کاربر تأیید نشده است
    /// </summary>
    [Display(Name = "ایمیل تأیید نشده", Description = "ایمیل کاربر نیاز به تأیید دارد")]
    EmailNotConfirmed,

    /// <summary>
    /// شماره تلفن کاربر تأیید نشده است
    /// </summary>
    [Display(Name = "شماره تلفن تأیید نشده", Description = "شماره تلفن کاربر نیاز به تأیید دارد")]
    PhoneNotConfirmed,

    /// <summary>
    /// توکن تأیید کننده منقضی شده است
    /// </summary>
    [Display(Name = "توکن منقضی", Description = "توکن تأیید منقضی شده است")]
    TokenExpired,

    /// <summary>
    /// توکن تأیید کننده نامعتبر است
    /// </summary>
    [Display(Name = "توکن نامعتبر", Description = "توکن تأیید نامعتبر است")]
    InvalidToken,

    /// <summary>
    /// نیاز به احراز هویت دو مرحله‌ای
    /// </summary>
    [Display(Name = "نیاز به 2FA", Description = "احراز هویت دو مرحله‌ای مورد نیاز است")]
    TwoFactorRequired,

    /// <summary>
    /// دستگاه کاربر غیرمعتبر یا ناشناخته است
    /// </summary>
    [Display(Name = "دستگاه غیرمعتبر", Description = "دستگاه کاربر غیرمعتبر است")]
    DeviceNotTrusted
}