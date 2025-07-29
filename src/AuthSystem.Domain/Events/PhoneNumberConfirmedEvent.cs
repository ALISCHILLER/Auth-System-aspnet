using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تأیید شماره تلفن کاربر
/// این رویداد هنگام تأیید شماره تلفن کاربر ایجاد می‌شود
/// </summary>
public class PhoneNumberConfirmedEvent : IDomainEvent
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
    public PhoneNumberConfirmedEvent(Guid userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
}