using System;

namespace AuthSystem.Domain.Common.Clock;

/// <summary>
/// پیاده‌سازی مدیریت زمان در دامنه
/// - استفاده از این کلاس به جای DateTime.UtcNow برای تست‌پذیری
/// - امکان تنظیم زمان برای تست‌ها
/// </summary>
public class DomainClock : ISystemClock
{
    private static readonly Lazy<DomainClock> _instance = new(() => new DomainClock());
    private DateTime? _fixedTime;

    /// <summary>
    /// نمونه سینگلتون از DomainClock
    /// </summary>
    public static DomainClock Instance => _instance.Value;

    /// <summary>
    /// تاریخ و زمان فعلی به صورت UTC
    /// </summary>
    public DateTime UtcNow => _fixedTime ?? DateTime.UtcNow;

    /// <summary>
    /// تنظیم زمان به مقدار مشخص (برای تست‌ها)
    /// </summary>
    public void SetFixedTime(DateTime utcNow)
    {
        _fixedTime = utcNow;
    }

    /// <summary>
    /// ریست کردن زمان به حالت پیش‌فرض
    /// </summary>
    public void Reset()
    {
        _fixedTime = null;
    }

    /// <summary>
    /// جلو انداختن زمان به میزان مشخص (برای تست‌ها)
    /// </summary>
    public void Advance(TimeSpan timeSpan)
    {
        if (_fixedTime.HasValue)
        {
            _fixedTime = _fixedTime.Value.Add(timeSpan);
        }
    }
}

/// <summary>
/// Façade برای دسترسی آسان به زمان سیستم
/// این کلاس استاتیک امکان دسترسی ساده‌تر به UtcNow را فراهم می‌کند
/// </summary>
public static class Clock
{
    /// <summary>
    /// دریافت زمان فعلی به صورت UTC
    /// </summary>
    public static DateTime UtcNow => DomainClock.Instance.UtcNow;
}