using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد غیرفعال شدن احراز هویت دو عاملی برای کاربر
/// </summary>
public sealed class TwoFactorDisabledEvent : DomainEvent
{
    public TwoFactorDisabledEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}