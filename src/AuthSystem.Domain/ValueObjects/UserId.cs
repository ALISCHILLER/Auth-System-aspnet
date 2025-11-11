using AuthSystem.Domain.Common.Base;

namespace AuthSystem.Domain.ValueObjects;

public sealed class UserId : ValueObject
{
    public UserId(Guid value)
    {
        Value = value != Guid.Empty ? value : throw new ArgumentException("UserId cannot be empty", nameof(value));
    }

    public Guid Value { get; }

    public static UserId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId userId) => userId.Value;
}