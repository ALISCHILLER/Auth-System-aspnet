using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Entities.UserAggregate.Rules;

/// <summary>
/// قانون اعتبارسنجی شماره تلفن کاربر
/// </summary>
public sealed class UserMustHaveValidPhoneRule : BusinessRuleBase
{
    private readonly string? _phoneNumber;

    public UserMustHaveValidPhoneRule(string? phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public override string Message => "شماره تلفن کاربر نامعتبر است";

    public override bool IsBroken()
    {
        if (string.IsNullOrWhiteSpace(_phoneNumber))
        {
            return true;
        }

        return !PhoneNumber.IsValidPhoneNumber(_phoneNumber);
    }
}