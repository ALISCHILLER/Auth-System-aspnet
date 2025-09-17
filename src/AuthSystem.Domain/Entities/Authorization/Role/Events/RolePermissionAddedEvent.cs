using AuthSystem.Domain.Common.Events;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Events;

/// <summary>
/// رویداد افزودن مجوز به نقش
/// </summary>
public sealed class RolePermissionAddedEvent : DomainEventBase
{
	public RolePermissionAddedEvent(Guid roleId, PermissionType permission)
	{
		RoleId = roleId;
		Permission = permission;
	}

	public Guid RoleId { get; }

	public PermissionType Permission { get; }
}