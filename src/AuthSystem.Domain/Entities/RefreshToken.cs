using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت توکن تازه‌سازی
/// این کلاس برای مدیریت جلسات کاربری و امکان تازه‌سازی توکن‌های دسترسی استفاده می‌شود
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// مقدار توکن (باید منحصر به فرد باشد)
    /// </summary>
    [Required]
    public string Token { get; private set; } = string.Empty;

    /// <summary>
    /// شناسه کاربر صاحب توکن
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// کاربر صاحب توکن
    /// </summary>
    public User User { get; private set; } = default!;

    /// <summary>
    /// تاریخ انقضای توکن
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// تاریخ ابطال توکن (در صورت ابطال)
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// آدرس IP که توکن از آن درخواست شده
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// اطلاعات User Agent (مرورگر/دستگاه)
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// آیا توکن هنوز فعال است؟
    /// </summary>
    public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;

    // متدهای دامنه‌ای

    /// <summary>
    /// ساخت یک توکن تازه‌سازی جدید
    /// </summary>
    /// <param name="token">مقدار توکن</param>
    /// <param name="expiresAt">تاریخ انقضا</param>
    /// <param name="ipAddress">آدرس IP</param>
    /// <param name="userAgent">اطلاعات User Agent</param>
    /// <returns>یک نمونه جدید از کلاس RefreshToken</returns>
    public static RefreshToken Create(string token, DateTime expiresAt, string? ipAddress, string? userAgent)
    {
        return new RefreshToken
        {
            Token = token,
            ExpiresAt = expiresAt,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }

    /// <summary>
    /// ابطال توکن
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }
}