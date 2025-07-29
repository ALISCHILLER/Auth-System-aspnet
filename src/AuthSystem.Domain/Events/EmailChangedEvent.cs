using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تغییر ایمیل کاربر
/// </summary>
public class EmailChangedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// ایمیل جدید
    /// </summary>
    public string NewEmail { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="newEmail">ایمیل جدید</param>
    public EmailChangedEvent(Guid userId, string newEmail)
    {
        UserId = userId;
        NewEmail = newEmail;
        OccurredOn = DateTime.UtcNow;
    }
}