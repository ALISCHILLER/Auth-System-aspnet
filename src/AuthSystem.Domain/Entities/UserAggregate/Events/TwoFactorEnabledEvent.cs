using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد فعال شدن احراز هویت دو عاملی برای کاربر
/// </summary>
public sealed class TwoFactorEnabledEvent : DomainEvent
{
    public TwoFactorEnabledEvent(Guid userId, TwoFactorSecretKey secretKey)
    {
        UserId = userId;
        SecretKey = secretKey;
    }

    public Guid UserId { get; }

    public TwoFactorSecretKey SecretKey { get; }
}