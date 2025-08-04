// AuthSystem.Domain/Exceptions/Infrastructure/
using System;

namespace AuthSystem.Domain.Exceptions.Infrastructure;

/// <summary>
/// Exception پایه برای خطاهای زیرساخت
/// </summary>
public abstract class InfrastructureException : Exception
{
    protected InfrastructureException(string message) : base(message) { }
    protected InfrastructureException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// زمانی که پیکربندی مورد نیاز در زیرساخت وجود ندارد
/// </summary>
public class ConfigurationMissingException : InfrastructureException
{
    public ConfigurationMissingException(string settingName) : base($"تنظیم '{settingName}' در پیکربندی یافت نشد.") { }
}

/// <summary>
/// زمانی که پیکربندی معتبر نیست
/// </summary>
public class InvalidConfigurationException : InfrastructureException
{
    public InvalidConfigurationException(string settingName, string reason) : base($"تنظیم '{settingName}' نامعتبر است: {reason}") { }
}

/// <summary>
/// زمانی که عملیات ارسال ایمیل با خطا مواجه می‌شود
/// </summary>
public class EmailSendingException : InfrastructureException
{
    public EmailSendingException(string to, string subject, Exception innerException)
        : base($"ارسال ایمیل به '{to}' با موضوع '{subject}' با خطا مواجه شد.", innerException) { }
}

/// <summary>
/// زمانی که عملیات ارسال پیامک با خطا مواجه می‌شود
/// </summary>
public class SmsSendingException : InfrastructureException
{
    public SmsSendingException(string to, string message, Exception innerException)
        : base($"ارسال پیامک به '{to}' با متن '{message}' با خطا مواجه شد.", innerException) { }
}