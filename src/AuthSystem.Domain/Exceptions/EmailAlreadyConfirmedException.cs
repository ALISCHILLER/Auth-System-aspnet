using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که ایمیل کاربر قبلاً تأیید شده است
/// </summary>
public class EmailAlreadyConfirmedException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده با شناسه کاربر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    public EmailAlreadyConfirmedException(Guid userId)
        : base($"ایمیل کاربر با شناسه {userId} قبلاً تأیید شده است")
    {
        UserId = userId;
    }
}