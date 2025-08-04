using AuthSystem.Domain.Interfaces.Services;
using System;

namespace AuthSystem.Infrastructure.Services;

/// <summary>
/// پیاده‌سازی IDateTimeProvider
/// این کلاس برای ارائه زمان استفاده می‌شود
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// دریافت زمان فعلی
    /// </summary>
    public DateTime Now => DateTime.Now;

    /// <summary>
    /// دریافت زمان فعلی به صورت UTC
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// دریافت زمان فعلی به صورت محلی
    /// </summary>
    public DateTime Today => DateTime.Today;
}