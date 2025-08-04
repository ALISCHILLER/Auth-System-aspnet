using System;

namespace AuthSystem.Domain.Entities;

/// <summary>
/// موجودیت ارتباط Many-to-Many بین Role و Permission
/// </summary>
public class RolePermission
{
    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// شناسه مجوز
    /// </summary>
    public Guid PermissionId { get; private set; }

    // Navigation Properties
    /// <summary>
    /// نقش مربوطه
    /// </summary>
    public Role Role { get; private set; } = default!;

    /// <summary>
    /// مجوز مربوطه
    /// </summary>
    public Permission Permission { get; private set; } = default!;

    // Required for EF Core
    private RolePermission() { }

    // Factory method
    public static RolePermission Create(Guid roleId, Guid permissionId)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };
    }
}