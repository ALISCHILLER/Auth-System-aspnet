namespace AuthSystem.Domain.Common.Exceptions;

/// <summary>
/// استثنا برای نقض سیاست‌ها
/// </summary>
public class PolicyViolationException : DomainException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "PolicyViolation";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public PolicyViolationException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public PolicyViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// ایجاد استثنا برای نقض سیاست
    /// </summary>
    public static PolicyViolationException ForPolicy(string policyName, string reason)
    {
        return new PolicyViolationException($"سیاست '{policyName}' نقض شده است: {reason}");
    }
}