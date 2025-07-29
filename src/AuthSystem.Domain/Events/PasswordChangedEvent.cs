using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تغییر رمز عبور کاربر
/// این رویداد هنگام تغییر رمز عبور کاربر ایجاد می‌شود
/// </summary>
public class PasswordChangedEvent : IDomainEvent
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
    public PasswordChangedEvent(Guid userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
}