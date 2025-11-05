using AuthSystem.Domain.Common.Events;

namespace AuthSystem.Domain.Entities.UserAggregate.Events;

/// <summary>
/// رویداد تغییر نقش کاربر
/// </summary>
public sealed class UserRoleChangedEvent : DomainEvent
{
    public UserRoleChangedEvent(Guid userId, IReadOnlyCollection<string> previousRoles, IReadOnlyCollection<string> currentRoles)
    {
        UserId = userId;
        PreviousRoles = previousRoles;
        CurrentRoles = currentRoles;
    }

    public Guid UserId { get; }

    public IReadOnlyCollection<string> PreviousRoles { get; }

    public IReadOnlyCollection<string> CurrentRoles { get; }
}