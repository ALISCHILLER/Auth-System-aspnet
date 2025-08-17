using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد تکمیل بازیابی رمز عبور
/// زمانی اتفاق می‌افتد که کاربر رمز عبور خود را بازیابی کند
/// </summary>
public class UserPasswordResetCompletedEvent : DomainEventBase
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// آدرس ایمیل
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// سازنده رویداد تکمیل بازیابی رمز عبور
    /// </summary>
    public UserPasswordResetCompletedEvent(
        Guid userId,
        string email,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
    }
}