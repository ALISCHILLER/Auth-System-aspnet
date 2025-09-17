using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون نیاز نقش به حداقل یک مجوز
/// </summary>
public sealed class RoleMustHavePermissionsRule : BusinessRuleBase
{
    private readonly IReadOnlyCollection<PermissionType> _permissions;

    public RoleMustHavePermissionsRule(IReadOnlyCollection<PermissionType> permissions)
    {
        _permissions = permissions ?? Array.Empty<PermissionType>();
    }

    public override string Message => "نقش باید حداقل یک مجوز داشته باشد";

    public override bool IsBroken()
    {
        return _permissions.Count == 0;
    }
}