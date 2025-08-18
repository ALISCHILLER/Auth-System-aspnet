using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد ورود موفق کاربر
/// زمانی اتفاق می‌افتد که کاربر با موفقیت وارد سیستم شود
/// </summary>
public class UserLoggedInEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// آدرس ایمیل کاربر
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// آدرس IP ورود
    /// </summary>
    public string IpAddress { get; }

    /// <summary>
    /// سازنده رویداد ورود
    /// </summary>
    public UserLoggedInEvent(
        Guid userId,
        string email,
        string ipAddress,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
        IpAddress = ipAddress;
    }
}