using System;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت تاریخچه ورود کاربر
/// برای ردیابی ورودهای کاربر به سیستم
/// </summary>
public class LoginHistory : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// آدرس IP ورود
    /// </summary>
    public IpAddress IpAddress { get; private set; }

    /// <summary>
    /// User Agent دستگاه
    /// </summary>
    public UserAgent UserAgent { get; private set; }

    /// <summary>
    /// نوع دستگاه
    /// </summary>
    public DeviceType DeviceType { get; private set; }

    /// <summary>
    /// آیا ورود موفق بود
    /// </summary>
    public bool IsSuccessful { get; private set; }

    /// <summary>
    /// زمان ورود
    /// </summary>
    public DateTime LoginAt { get; private set; }

    /// <summary>
    /// زمان خروج (در صورت وجود)
    /// </summary>
    public DateTime? LogoutAt { get; private set; }

    /// <summary>
    /// مدت زمان جلسه (در صورت پایان یافتن)
    /// </summary>
    public TimeSpan? Duration => LogoutAt.HasValue ? LogoutAt.Value - LoginAt : null;

    /// <summary>
    /// سازنده خصوصی برای ORM
    /// </summary>
    private LoginHistory() { }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    private LoginHistory(
        Guid userId,
        IpAddress ipAddress,
        UserAgent userAgent,
        bool isSuccessful)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        DeviceType = userAgent.DeviceType;
        IsSuccessful = isSuccessful;
        LoginAt = DateTime.UtcNow;
    }

    /// <summary>
    /// ایجاد نمونه جدید تاریخچه ورود
    /// </summary>
    public static LoginHistory Create(
        Guid userId,
        IpAddress ipAddress,
        UserAgent userAgent,
        bool isSuccessful)
    {
        return new LoginHistory(userId, ipAddress, userAgent, isSuccessful);
    }

    /// <summary>
    /// ثبت زمان خروج
    /// </summary>
    public void RecordLogout()
    {
        LogoutAt = DateTime.UtcNow;
    }

    /// <summary>
    /// بررسی آیا این ورود هنوز فعال است
    /// </summary>
    public bool IsActive => !LogoutAt.HasValue;

    /// <summary>
    /// تمدید زمان ورود (برای جلسه‌های فعال)
    /// </summary>
    public void ExtendSession(TimeSpan extension)
    {
        // در عمل ممکن است نیاز به به‌روزرسانی LoginAt داشته باشید
    }
}