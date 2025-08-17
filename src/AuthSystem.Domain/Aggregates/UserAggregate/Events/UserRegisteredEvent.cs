using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Events;

/// <summary>
/// رویداد ثبت‌نام کاربر جدید
/// زمانی اتفاق می‌افتد که کاربر جدیدی در سیستم ثبت‌نام کند
/// </summary>
public class UserRegisteredEvent : DomainEventBase
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
    /// شماره تلفن کاربر (اختیاری)
    /// </summary>
    public string? PhoneNumber { get; }

    /// <summary>
    /// سازنده رویداد ثبت‌نام
    /// </summary>
    public UserRegisteredEvent(
        Guid userId,
        string email,
        string? phoneNumber = null,
        string? triggeredBy = null)
        : base(triggeredBy)
    {
        UserId = userId;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}