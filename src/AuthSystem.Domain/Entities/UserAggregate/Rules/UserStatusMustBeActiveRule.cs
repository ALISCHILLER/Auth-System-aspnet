using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون الزام وضعیت فعال کاربر برای انجام عملیات
/// </summary>
public sealed class UserStatusMustBeActiveRule : BusinessRuleBase
{
    private readonly UserStatus _status;

    public UserStatusMustBeActiveRule(UserStatus status)
    {
        _status = status;
    }

    public override string Message => "وضعیت کاربر باید فعال باشد";

    public override bool IsBroken()
    {
        return _status != UserStatus.Active;
    }
}