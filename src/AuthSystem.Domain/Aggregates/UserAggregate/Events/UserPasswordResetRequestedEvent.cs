using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد درخواست بازیابی رمز عبور
/// زمانی اتفاق می‌افتد که کاربر درخواست بازیابی رمز عبور دهد
/// </summary>
public class UserPasswordResetRequestedEvent : DomainEventBase
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
    /// مقدار توکن
    /// </summary>
    public string TokenValue { get; }

    /// <summary>
    /// سازنده رویداد درخواست بازیابی رمز عبور
    /// </summary>
    public UserPasswordResetRequestedEvent(
        Guid userId,
        string email,
        string tokenValue,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
        TokenValue = tokenValue;
    }
}