// File: AuthSystem.Domain/Common/Clock/DomainClock.cs
using System;
using AuthSystem.Domain.Common.Clock;

namespace AuthSystem.Domain.Common.Clock
{
    /// <summary>
    /// نگهدارندهٔ سراسریِ ساعت دامنه برای تست‌پذیری و جداسازی از زمان سیستم.
    /// - در Infrastructure یک ISystemClock واقعی ست می‌شود.
    /// - در تست‌ها می‌توانید Fake/Stub تزریق کنید.
    /// </summary>
    public static class DomainClock
    {
        private static ISystemClock _current = new DefaultSystemClock();
        private static bool _isConfigured = false;

        /// <summary>
        /// زمان فعلی UTC بر اساس ساعت جاری
        /// </summary>
        public static DateTime UtcNow => _current.UtcNow;

        /// <summary>
        /// زمان فعلی Local بر اساس ساعت جاری
        /// </summary>
        public static DateTime Now => _current.Now;

        /// <summary>
        /// تنظیم ساعت دامنه (فقط یک بار قابل فراخوانی)
        /// </summary>
        public static void Configure(ISystemClock clock)
        {
            if (_isConfigured)
                throw new InvalidOperationException("ساعت دامنه قبلاً تنظیم شده است");

            _current = clock ?? throw new ArgumentNullException(nameof(clock));
            _isConfigured = true;
        }

        /// <summary>
        /// تنظیم مجدد به ساعت پیش‌فرض
        /// </summary>
        public static void Reset()
        {
            _current = new DefaultSystemClock();
            _isConfigured = false;
        }

        /// <summary>
        /// ساعت پیش‌فرض سیستم
        /// </summary>
        private sealed class DefaultSystemClock : ISystemClock
        {
            public DateTime UtcNow => DateTime.UtcNow;
            public DateTime Now => DateTime.Now;
        }
    }
}