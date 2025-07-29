using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت تاریخچه ورود
/// این کلاس برای ثبت تاریخچه ورود کاربران به سیستم استفاده می‌شود
/// </summary>
public class LoginHistory : BaseEntity
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// کاربر مربوطه
    /// </summary>
    public User User { get; private set; } = default!;

    /// <summary>
    /// تاریخ و زمان ورود
    /// مقدار پیش‌فرض: زمان فعلی سیستم
    /// </summary>
    public DateTime LoginAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// آدرس IP که از آن وارد شده
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// اطلاعات User Agent (مرورگر/دستگاه)
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// آیا ورود موفق بوده؟
    /// </summary>
    public bool IsSuccessful { get; private set; }

    /// <summary>
    /// دلیل ناموفق بودن ورود (در صورت وجود)
    /// </summary>
    public string? FailureReason { get; private set; }

    // متدهای دامنه‌ای

    /// <summary>
    /// ساخت یک ورود موفق جدید
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="ipAddress">آدرس IP</param>
    /// <param name="userAgent">اطلاعات User Agent</param>
    /// <returns>یک نمونه جدید از کلاس LoginHistory</returns>
    public static LoginHistory CreateSuccess(Guid userId, string? ipAddress, string? userAgent)
    {
        return new LoginHistory
        {
            UserId = userId,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsSuccessful = true
        };
    }

    /// <summary>
    /// ساخت یک ورود ناموفق جدید
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="ipAddress">آدرس IP</param>
    /// <param name="userAgent">اطلاعات User Agent</param>
    /// <param name="failureReason">دلیل ناموفق بودن</param>
    /// <returns>یک نمونه جدید از کلاس LoginHistory</returns>
    public static LoginHistory CreateFailure(Guid userId, string? ipAddress, string? userAgent, string failureReason)
    {
        return new LoginHistory
        {
            UserId = userId,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsSuccessful = false,
            FailureReason = failureReason
        };
    }
}