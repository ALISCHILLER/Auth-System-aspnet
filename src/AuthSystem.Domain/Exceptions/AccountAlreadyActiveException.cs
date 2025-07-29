using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که حساب کاربری قبلاً فعال است
/// </summary>
public class AccountAlreadyActiveException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده با شناسه کاربر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    public AccountAlreadyActiveException(Guid userId)
        : base($"حساب کاربری با شناسه {userId} قبلاً فعال است")
    {
        UserId = userId;
    }
}