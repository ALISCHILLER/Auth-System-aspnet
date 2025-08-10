using AuthSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace AuthSystem.Application.DTOs.Responses;



/// <summary>
/// مدل پاسخ شامل اطلاعات کاربر
/// </summary>
public class UserResponse
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// آدرس ایمیل
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// آیا ایمیل تأیید شده است؟
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// شماره تلفن
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// آیا شماره تلفن تأیید شده است؟
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// کد ملی
    /// </summary>
    public string NationalCode { get; set; } = null!;

    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// آیا کاربر فعال است؟
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// تاریخ و زمان آخرین ورود
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// تاریخ ایجاد کاربر
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// آدرس URL تصویر پروفایل
    /// </summary>
    public string? ProfileImageUrl { get; set; }

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
}