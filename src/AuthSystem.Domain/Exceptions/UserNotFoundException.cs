using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که کاربری با شناسه مورد نظر پیدا نشد
/// </summary>
public class UserNotFoundException : DomainException
{
    /// <summary>
    /// شناسه کاربر که پیدا نشد
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// سازنده با شناسه کاربر
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    public UserNotFoundException(Guid userId)
        : base($"کاربری با شناسه {userId} یافت نشد")
    {
        UserId = userId;
    }

    /// <summary>
    /// سازنده با شناسه کاربر و پیام خطا
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="message">پیام خطا</param>
    public UserNotFoundException(Guid userId, string message)
        : base(message)
    {
        UserId = userId;
    }
}