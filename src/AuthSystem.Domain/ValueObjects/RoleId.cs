using AuthSystem.Domain.Common.Base;

namespace AuthSystem.Domain.ValueObjects;

public sealed class RoleId : ValueObject
{
    public RoleId(Guid value)
    {
        Value = value != Guid.Empty ? value : throw new ArgumentException("RoleId cannot be empty", nameof(value));
    }

    public Guid Value { get; }

    public static RoleId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(RoleId roleId) => roleId.Value;
}