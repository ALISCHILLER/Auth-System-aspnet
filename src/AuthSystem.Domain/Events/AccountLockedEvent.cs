using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد قفل شدن حساب کاربری
/// این رویداد هنگام قفل شدن حساب کاربری ایجاد می‌شود
/// </summary>
public class AccountLockedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// زمان قفل شدن حساب
    /// </summary>
    public DateTime LockedAt { get; }

    /// <summary>
    /// زمان پایان قفل (در صورت وجود)
    /// </summary>
    public DateTime? LockoutEnd { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="lockedAt">زمان قفل شدن</param>
    /// <param name="lockoutEnd">زمان پایان قفل</param>
    public AccountLockedEvent(Guid userId, DateTime lockedAt, DateTime? lockoutEnd = null)
    {
        UserId = userId;
        LockedAt = lockedAt;
        LockoutEnd = lockoutEnd;
        OccurredOn = DateTime.UtcNow;
    }
}