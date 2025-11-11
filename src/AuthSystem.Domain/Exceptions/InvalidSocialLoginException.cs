using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// Exception that represents invalid social login assignments within the user aggregate.
/// </summary>
public sealed class InvalidSocialLoginException : DomainException
{
    private InvalidSocialLoginException(string message)
        : base(message)
    {
    }

    private InvalidSocialLoginException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates an exception when the provider name is missing or empty.
    /// </summary>
    public static InvalidSocialLoginException ForMissingProvider(Guid userId)
    {
        return new InvalidSocialLoginException($"ورود اجتماعی برای کاربر '{userId}' به دلیل نام ارائه‌دهنده نامعتبر است.");
    }

    /// <summary>
    /// Creates an exception when the provider user identifier is missing or empty.
    /// </summary>
    public static InvalidSocialLoginException ForMissingProviderUserId(Guid userId, string provider)
    {
        return new InvalidSocialLoginException($"شناسه کاربر ارائه‌دهنده '{provider}' برای کاربر '{userId}' نامعتبر است.");
    }
}