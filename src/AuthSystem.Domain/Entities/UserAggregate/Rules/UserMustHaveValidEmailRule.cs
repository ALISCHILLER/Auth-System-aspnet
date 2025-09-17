using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی ایمیل کاربر
/// </summary>
public sealed class UserMustHaveValidEmailRule : BusinessRuleBase
{
    private readonly string? _email;

    public UserMustHaveValidEmailRule(string? email)
    {
        _email = email;
    }

    public override string Message => "ایمیل کاربر نامعتبر است";

    public override bool IsBroken()
    {
        if (string.IsNullOrWhiteSpace(_email))
        {
            return true;
        }

        return !Email.IsValidEmail(_email);
    }
}