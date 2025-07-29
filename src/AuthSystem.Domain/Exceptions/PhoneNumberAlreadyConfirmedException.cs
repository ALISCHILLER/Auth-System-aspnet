using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که شماره تلفن کاربر قبلاً تأیید شده است
/// </summary>
public class PhoneNumberAlreadyConfirmedException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده با شناسه کاربر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    public PhoneNumberAlreadyConfirmedException(Guid userId)
        : base($"شماره تلفن کاربر با شناسه {userId} قبلاً تأیید شده است")
    {
        UserId = userId;
    }
}