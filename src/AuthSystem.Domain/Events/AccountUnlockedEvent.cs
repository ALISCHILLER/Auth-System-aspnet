using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد باز شدن حساب کاربری قفل شده
/// این رویداد هنگام باز شدن حساب کاربری قفل شده ایجاد می‌شود
/// </summary>
public class AccountUnlockedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// زمان باز شدن حساب
    /// </summary>
    public DateTime UnlockedAt { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="unlockedAt">زمان باز شدن حساب</param>
    public AccountUnlockedEvent(Guid userId, DateTime unlockedAt)
    {
        UserId = userId;
        UnlockedAt = unlockedAt;
        OccurredOn = DateTime.UtcNow;
    }
}