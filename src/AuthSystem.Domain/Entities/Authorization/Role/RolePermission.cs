using System;
using AuthSystem.Domain.Common;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role;

/// <summary>
/// Entity برای مجوزهای نقش
/// </summary>
public class RolePermission : BaseEntity<Guid>
{
    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid RoleId { get; }

    /// <summary>
    /// نوع مجوز
    /// </summary>
    public PermissionType PermissionType { get; }

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// سازنده خصوصی
    /// </summary>
    private RolePermission()
    {
        // برای EF Core
    }

    /// <summary>
    /// سازنده اصلی
    /// </summary>
    public RolePermission(
        Guid id,
        Guid roleId,
        PermissionType permissionType) : base(id)
    {
        RoleId = roleId;
        PermissionType = permissionType;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// آیا این مجوز از نوع خاصی است
    /// </summary>
    public bool IsOfType(PermissionType permissionType)
    {
        return PermissionType == permissionType;
    }
}