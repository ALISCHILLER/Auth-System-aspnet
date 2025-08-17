using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای کلید مخفی 2FA نامعتبر
/// </summary>
[Serializable]
public sealed class InvalidTwoFactorSecretKeyException : DomainException
{
    private const string DEFAULT_ERROR_CODE = "AUTH.2FA.SECRET_KEY.INVALID";

    private InvalidTwoFactorSecretKeyException(string message, string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode)
    {
    }

    private InvalidTwoFactorSecretKeyException(string message, Exception innerException, string errorCode = DEFAULT_ERROR_CODE)
        : base(message, errorCode, innerException)
    {
    }

    /// <summary>
    /// استثنا برای طول نامعتبر کلید
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInvalidLength(int actualLength, int expectedLength)
    {
        var exception = new InvalidTwoFactorSecretKeyException(
            $"طول کلید نامعتبر است. طول فعلی: {actualLength}، طول مورد انتظار: {expectedLength}",
            "AUTH.2FA.SECRET_KEY.INVALID_LENGTH"
        );

        exception.WithDetail("ActualLength", actualLength);
        exception.WithDetail("ExpectedLength", expectedLength);

        return exception;
    }

    /// <summary>
    /// استثنا برای فرمت نامعتبر
    /// </summary>
    public static InvalidTwoFactorSecretKeyException ForInvalidFormat(string? value, Exception? innerException = null)
    {
        var message = "فرمت کلید مخفی نامعتبر است";

        if (innerException != null)
        {
            return new InvalidTwoFactorSecretKeyException(
                message,
                innerException,
                "AUTH.2FA.SECRET_KEY.INVALID_FORMAT"
            );
        }

        return new InvalidTwoFactorSecretKeyException(
            message,
            "AUTH.2FA.SECRET_KEY.INVALID_FORMAT"
        );
    }
}
