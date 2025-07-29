using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تغییر شماره تلفن کاربر
/// </summary>
public class PhoneNumberChangedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// شماره تلفن جدید
    /// </summary>
    public string NewPhoneNumber { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="newPhoneNumber">شماره تلفن جدید</param>
    public PhoneNumberChangedEvent(Guid userId, string newPhoneNumber)
    {
        UserId = userId;
        NewPhoneNumber = newPhoneNumber;
        OccurredOn = DateTime.UtcNow;
    }
}