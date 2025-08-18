using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد قفل شدن حساب کاربر
/// زمانی اتفاق می‌افتد که حساب کاربر به دلیل تلاش‌های ناموفق متعدد قفل شود
/// </summary>
public class UserLockedEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// دلیل قفل شدن حساب
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// سازنده رویداد قفل شدن
    /// </summary>
    public UserLockedEvent(
        Guid userId,
        string reason,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Reason = reason;
    }
}