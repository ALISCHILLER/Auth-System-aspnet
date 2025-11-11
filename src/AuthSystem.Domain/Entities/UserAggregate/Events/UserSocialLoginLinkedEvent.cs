using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// Event emitted whenever a social login provider is linked to a user.
/// </summary>
public sealed class UserSocialLoginLinkedEvent : DomainEvent
{
    public UserSocialLoginLinkedEvent(Guid userId, string provider, string providerUserId)
    {
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
    }

    public Guid UserId { get; }

    public string Provider { get; }

    public string ProviderUserId { get; }
}