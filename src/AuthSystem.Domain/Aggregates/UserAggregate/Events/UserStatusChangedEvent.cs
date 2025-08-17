using AuthSystem.Domain.Common;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد تغییر وضعیت کاربر
/// زمانی اتفاق می‌افتد که وضعیت کاربر تغییر کند
/// </summary>
public class UserStatusChangedEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// وضعیت قبلی
    /// </summary>
    public UserStatus OldStatus { get; }

    /// <summary>
    /// وضعیت جدید
    /// </summary>
    public UserStatus NewStatus { get; }

    /// <summary>
    /// دلیل تغییر وضعیت
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// سازنده رویداد تغییر وضعیت
    /// </summary>
    public UserStatusChangedEvent(
        Guid userId,
        UserStatus oldStatus,
        UserStatus newStatus,
        string reason,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        Reason = reason;
    }
}