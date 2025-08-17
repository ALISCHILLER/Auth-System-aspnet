using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد غیرفعال‌سازی احراز هویت دو عاملی
/// زمانی اتفاق می‌افتد که کاربر احراز هویت دو عاملی را غیرفعال کند
/// </summary>
public class TwoFactorDisabledEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده رویداد غیرفعال‌سازی 2FA
    /// </summary>
    public TwoFactorDisabledEvent(
        Guid userId,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
    }
}