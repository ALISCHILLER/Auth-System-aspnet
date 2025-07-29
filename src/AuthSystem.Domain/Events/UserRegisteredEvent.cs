using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد ثبت‌نام کاربر جدید
/// این رویداد هنگام ثبت‌نام موفق کاربر جدید ایجاد می‌شود
/// </summary>
public class UserRegisteredEvent : IDomainEvent
{
    /// <summary>
    /// شناسه کاربر ثبت‌نام‌کننده
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// نام کاربری کاربر
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// زمان وقوع رویداد
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// سازنده رویداد
    /// </summary>
    /// <param name="userId">شناسه کاربر</param>
    /// <param name="email">آدرس ایمیل</param>
    /// <param name="userName">نام کاربری</param>
    public UserRegisteredEvent(Guid userId, string email, string userName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
        OccurredOn = DateTime.UtcNow;
    }
}