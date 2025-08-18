// File: AuthSystem.Domain/Common/Clock/ISystemClock.cs
using System;

namespace AuthSystem.Domain.Common.Clock
{
    /// <summary>
    /// قرارداد ساعت سیستم در دامنه
    /// - برای تست‌پذیری، زمان از طریق این اینترفیس تزریق می‌شود
    /// - پیاده‌سازی در لایه زیرساخت انجام می‌گردد (مثلاً SystemClock)
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>زمان فعلی UTC</summary>
        DateTime UtcNow { get; }

        /// <summary>زمان فعلی به‌صورت Local (در صورت نیاز)</summary>
        DateTime Now { get; }
    }
}