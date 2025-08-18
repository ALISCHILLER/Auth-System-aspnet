using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد تأیید ایمیل
/// زمانی اتفاق می‌افتد که ایمیل کاربر تأیید شود
/// </summary>
public class EmailVerifiedEvent : DomainEventBase
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
    /// سازنده رویداد تأیید ایمیل
    /// </summary>
    public EmailVerifiedEvent(
        Guid userId,
        string email,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
    }
}