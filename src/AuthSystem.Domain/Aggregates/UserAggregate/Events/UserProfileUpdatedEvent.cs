using System.Collections.Generic;
using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد به‌روزرسانی پروفایل کاربر
/// زمانی اتفاق می‌افتد که اطلاعات پروفایل کاربر تغییر کند
/// </summary>
public class UserProfileUpdatedEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// فیلدهای به‌روزرسانی شده
    /// </summary>
    public IReadOnlyList<string> UpdatedFields { get; }

    /// <summary>
    /// سازنده رویداد به‌روزرسانی پروفایل
    /// </summary>
    public UserProfileUpdatedEvent(
        Guid userId,
        IEnumerable<string> updatedFields,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        UpdatedFields = new List<string>(updatedFields);
    }
}