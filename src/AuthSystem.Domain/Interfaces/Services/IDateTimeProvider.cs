using System;

namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط برای ارائه زمان
/// این رابط برای تست‌پذیری بهتر و کنترل زمان استفاده می‌شود
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// دریافت زمان فعلی
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// دریافت زمان فعلی به صورت UTC
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// دریافت زمان فعلی به صورت محلی
    /// </summary>
    DateTime Today { get; }
}