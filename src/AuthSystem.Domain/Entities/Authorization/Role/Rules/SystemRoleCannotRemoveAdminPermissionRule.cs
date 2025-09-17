using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون جلوگیری از حذف مجوز ادمین از نقش‌های سیستمی
/// </summary>
public sealed class SystemRoleCannotRemoveAdminPermissionRule : BusinessRuleBase
{
    private readonly bool _isSystemRole;
    private readonly PermissionType _permission;

    public SystemRoleCannotRemoveAdminPermissionRule(bool isSystemRole, PermissionType permission)
    {
        _isSystemRole = isSystemRole;
        _permission = permission;
    }

    public override string Message => "مجوز ادمین از نقش‌های سیستمی قابل حذف نیست";

    public override bool IsBroken()
    {
        return _isSystemRole && _permission == PermissionType.Admin;
    }
}