using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تأیید ایمیل کاربر
/// این رویداد هنگام تأیید ایمیل کاربر ایجاد می‌شود
/// </summary>
public class EmailConfirmedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    public EmailConfirmedEvent(Guid userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
}