using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون جلوگیری از تکرار مجوزها در نقش
/// </summary>
public sealed class RoleCannotHaveDuplicatePermissionsRule : BusinessRuleBase
{
    private readonly IEnumerable<PermissionType> _existingPermissions;
    private readonly PermissionType _permissionToAdd;

    public RoleCannotHaveDuplicatePermissionsRule(
        IEnumerable<PermissionType> existingPermissions,
        PermissionType permissionToAdd)
    {
        _existingPermissions = existingPermissions ?? Array.Empty<PermissionType>();
        _permissionToAdd = permissionToAdd;
    }

    public override string Message => $"مجوز {_permissionToAdd} قبلاً به نقش اضافه شده است";

    public override bool IsBroken()
    {
        return _existingPermissions.Contains(_permissionToAdd);
    }
}