using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد ورود موفق کاربر
/// </summary>
public sealed class UserLoggedInEvent : DomainEventBase
{
    public UserLoggedInEvent(Guid userId, IpAddress? ipAddress, UserAgent? userAgent)
    {
        UserId = userId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public Guid UserId { get; }

    public IpAddress? IpAddress { get; }

    public UserAgent? UserAgent { get; }
}