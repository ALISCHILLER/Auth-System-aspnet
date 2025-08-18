using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد باز شدن قفل حساب کاربر
/// زمانی اتفاق می‌افتد که قفل حساب کاربر باز شود
/// </summary>
public class UserUnlockedEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده رویداد باز شدن قفل
    /// </summary>
    public UserUnlockedEvent(
        Guid userId,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
    }
}