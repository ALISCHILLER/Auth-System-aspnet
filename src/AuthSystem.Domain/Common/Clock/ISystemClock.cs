using System;

namespace AuthSystem.Domain.Common.Clock;

/// <summary>
/// اینترفیس برای مدیریت زمان
/// - استفاده از این اینترفیس به جای DateTime.UtcNow برای تست‌پذیری
/// </summary>
public interface ISystemClock
{
    /// <summary>
    /// تاریخ و زمان فعلی به صورت UTC
    /// </summary>
    DateTime UtcNow { get; }
}