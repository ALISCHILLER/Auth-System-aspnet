using System;
using System.Collections.Generic;

namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ ورود به سیستم
/// شامل توکن دسترسی، توکن تازه‌سازی و اطلاعات کاربر
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// توکن JWT برای دسترسی به APIها
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// توکن تازه‌سازی برای دریافت توکن جدید پس از انقضا
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// تاریخ و زمان انقضای توکن دسترسی
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// تاریخ و زمان انقضای توکن تازه‌سازی
    /// </summary>
    public DateTime? RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// نام کاربری کاربر
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// آیا ایمیل تأیید شده است؟
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// آیا شماره تلفن تأیید شده است؟
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// لیست نقش‌های کاربر
    /// </summary>
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// لیست مجوزهای کاربر
    /// </summary>
    public IReadOnlyCollection<string> Permissions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// وضعیت احراز هویت دو مرحله‌ای
    /// </summary>
    public bool TwoFactorAuthEnabled { get; set; }

    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    public string Gender { get; set; } = null!;

    /// <summary>
    /// آدرس URL تصویر پروفایل کاربر
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}