using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Mocks;

/// <summary>
/// شبیه‌سازی مدیریت زمان برای تست‌ها
/// </summary>
public class MockClock : ISystemClock
{
    /// <summary>
    /// زمان فعلی شبیه‌سازی شده
    /// </summary>
    public DateTime UtcNow { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// تنظیم زمان به مقدار مشخص
    /// </summary>
    public void SetUtcNow(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    /// <summary>
    /// جلو انداختن زمان به میزان مشخص
    /// </summary>
    public void Advance(TimeSpan timeSpan)
    {
        UtcNow = UtcNow.Add(timeSpan);
    }

    /// <summary>
    /// تنظیم زمان به زمان فعلی سیستم
    /// </summary>
    public void Reset()
    {
        UtcNow = DateTime.UtcNow;
    }
}