using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون جلوگیری از ورود کاربر در هنگام قفل بودن حساب
/// </summary>
public sealed class UserCannotLoginWhenLockedRule : BusinessRuleBase
{
    private readonly DateTime? _lockoutEnd;

    public UserCannotLoginWhenLockedRule(DateTime? lockoutEnd)
    {
        _lockoutEnd = lockoutEnd;
    }

    public override string Message => "حساب کاربر قفل است";

    public override bool IsBroken()
    {
        return _lockoutEnd.HasValue && _lockoutEnd.Value > DateTime.UtcNow;
    }
}