using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون جلوگیری از تکرار نقش برای کاربر
/// </summary>
public sealed class UserRoleCannotBeDuplicatedRule : BusinessRuleBase
{
    private readonly IEnumerable<Guid> _existingRoleIds;
    private readonly Guid _roleId;

    public UserRoleCannotBeDuplicatedRule(IEnumerable<Guid> existingRoleIds, Guid roleId)
    {
        _existingRoleIds = existingRoleIds ?? Array.Empty<Guid>();
        _roleId = roleId;
    }

    public override string Message => "کاربر قبلاً این نقش را دارد";

    public override bool IsBroken()
    {
        return _existingRoleIds.Contains(_roleId);
    }
}