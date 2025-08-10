using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// انواع تأیید در سیستم
/// این enum برای مشخص کردن نوع تأیید (ایمیل، شماره تلفن، 2FA) استفاده می‌شود
/// </summary>
public enum ConfirmationType
{
    /// <summary>
    /// تأیید ایمیل
    /// </summary>
    [Display(Name = "ایمیل", Description = "تأیید آدرس ایمیل کاربر")]
    Email = 0,

    /// <summary>
    /// تأیید شماره تلفن
    /// </summary>
    [Display(Name = "شماره تلفن", Description = "تأیید شماره تلفن کاربر")]
    Phone = 1,

    /// <summary>
    /// تأیید احراز هویت دو مرحله‌ای
    /// </summary>
    [Display(Name = "2FA", Description = "تأیید کد احراز هویت دو مرحله‌ای")]
    TwoFactor = 2
}