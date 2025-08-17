using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد فعال‌سازی احراز هویت دو عاملی
/// زمانی اتفاق می‌افتد که کاربر احراز هویت دو عاملی را فعال کند
/// </summary>
public class TwoFactorEnabledEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده رویداد فعال‌سازی 2FA
    /// </summary>
    public TwoFactorEnabledEvent(
        Guid userId,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
    }
}