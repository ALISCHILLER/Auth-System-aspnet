using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// قانون اعتبارسنجی توضیحات نقش
/// </summary>
public sealed class RoleDescriptionMustBeValidRule : BusinessRuleBase
{
    private readonly string _description;

    public RoleDescriptionMustBeValidRule(string description)
    {
        _description = description;
    }

    public override string Message => "توضیحات نقش باید بین 3 تا 250 کاراکتر باشد";

    public override bool IsBroken()
    {
        if (_description is null)
        {
            return true;
        }

        var trimmed = _description.Trim();
        return trimmed.Length < 3 || trimmed.Length > 250;
    }
}