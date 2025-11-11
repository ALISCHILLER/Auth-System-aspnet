using AuthSystem.Domain.Common.Specifications;
using AuthSystem.Domain.Enums;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Entities.Authorization.Role.Specifications;

/// <summary>
/// مشخصه جستجوی نقش بر اساس مجموعه‌ای از مجوزها
/// </summary>
public sealed class RoleByPermissionsSpecification : BaseSpecification<Role>
{
    public RoleByPermissionsSpecification(IEnumerable<PermissionType> permissions)
        : base(BuildCriteria(permissions))
    {
    }

    private static Expression<Func<Role, bool>> BuildCriteria(IEnumerable<PermissionType> permissions)
    {
        var permissionArray = permissions?.Distinct().ToArray() ?? Array.Empty<PermissionType>();
        return role => permissionArray.All(role.HasPermission);
    }
}