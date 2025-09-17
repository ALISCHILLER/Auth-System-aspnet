using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی کد ملی کاربر
/// </summary>
public sealed class UserMustHaveValidNationalCodeRule : BusinessRuleBase
{
    private readonly string? _nationalCode;

    public UserMustHaveValidNationalCodeRule(string? nationalCode)
    {
        _nationalCode = nationalCode;
    }

    public override string Message => "کد ملی وارد شده نامعتبر است";

    public override bool IsBroken()
    {
        if (string.IsNullOrWhiteSpace(_nationalCode))
        {
            return false;
        }

        try
        {
            NationalCode.Create(_nationalCode);
            return false;
        }
        catch
        {
            return true;
        }
    }
}