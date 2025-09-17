using AuthSystem.Domain.Common.Rules;

namespace AuthSystem.Domain.Entities.Authorization.Role.Rules;

/// <summary>
/// ﬁ«‰Ê‰ „‰Õ’— »Â ›—œ »Êœ‰ ‰«„ ‰ﬁ‘
/// </summary>
public sealed class RoleNameUniqueRule : BusinessRuleBase
{
    private readonly string _name;
    private readonly Func<string, bool> _nameExists;

    public RoleNameUniqueRule(string name, Func<string, bool> nameExists)
    {
        _name = name;
        _nameExists = nameExists ?? throw new ArgumentNullException(nameof(nameExists));
    }

    public override string Message => $"‰«„ ‰ﬁ‘ '{_name}' ﬁ»·« À»  ‘œÂ «” ";

    public override bool IsBroken()
    {
        return _nameExists(_name);
    }
}