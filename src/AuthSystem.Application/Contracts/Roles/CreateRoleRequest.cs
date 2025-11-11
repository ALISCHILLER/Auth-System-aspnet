using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Contracts.Roles;

public sealed class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsSystemRole { get; set; }
    public IReadOnlyCollection<PermissionType> Permissions { get; set; } = Array.Empty<PermissionType>();
}