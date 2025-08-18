using AuthSystem.Domain.Common.Entities;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object برای نگهداری رمز عبور هش شده
/// </summary>
public sealed class PasswordHash : ValueObject
{
    public string Value { get; }

    private PasswordHash(string value) => Value = value;

    public static PasswordHash Create(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("هش نمی‌تواند خالی باشد");

        return new PasswordHash(hashedPassword);
    }

    public static PasswordHash FromPassword(Password password)
        => new(password.HashedValue);

    public bool VerifyPassword(string plainTextPassword)
    {
        var password = Password.CreateFromHash(Value);
        return password.Verify(plainTextPassword);
    }

    protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };
    public override string ToString() => "***HASH***";
}