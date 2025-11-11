using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون جلوگیری از خالی شدن نقش‌های پیش‌فرض
/// </summary>
public sealed class DefaultRoleCannotBeEmptyRule : BusinessRuleBase
{
    private readonly bool _isDefaultRole;
    private readonly int _remainingUsers;

    public DefaultRoleCannotBeEmptyRule(bool isDefaultRole, int remainingUsers)
    {
        _isDefaultRole = isDefaultRole;
        _remainingUsers = remainingUsers;
    }

    public override string Message => "نقش پیش‌فرض باید حداقل یک کاربر داشته باشد";

    public override bool IsBroken()
    {
        return _isDefaultRole && _remainingUsers <= 0;
    }
}