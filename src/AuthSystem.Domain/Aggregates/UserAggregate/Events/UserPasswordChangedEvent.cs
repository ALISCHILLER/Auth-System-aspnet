using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد تغییر رمز عبور
/// زمانی اتفاق می‌افتد که رمز عبور کاربر تغییر کند
/// </summary>
public class UserPasswordChangedEvent : DomainEventBase
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
    /// سازنده رویداد تغییر رمز عبور
    /// </summary>
    public UserPasswordChangedEvent(
        Guid userId,
        string email,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
    }
}