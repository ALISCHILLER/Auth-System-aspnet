using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی رمز عبور کاربر
/// </summary>
public sealed class UserMustHaveValidPasswordRule : BusinessRuleBase
{
    private readonly PasswordHash? _passwordHash;

    public UserMustHaveValidPasswordRule(PasswordHash? passwordHash)
    {
        _passwordHash = passwordHash;
    }

    public override string Message => "رمز عبور کاربر نامعتبر است";

    public override bool IsBroken()
    {
        return _passwordHash is null;
    }
}
