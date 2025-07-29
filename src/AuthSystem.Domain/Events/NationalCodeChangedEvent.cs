using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد تغییر کد ملی کاربر
/// </summary>
public class NationalCodeChangedEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// کد ملی جدید
    /// </summary>
    public string NewNationalCode { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="newNationalCode">کد ملی جدید</param>
    public NationalCodeChangedEvent(Guid userId, string newNationalCode)
    {
        UserId = userId;
        NewNationalCode = newNationalCode;
        OccurredOn = DateTime.UtcNow;
    }
}