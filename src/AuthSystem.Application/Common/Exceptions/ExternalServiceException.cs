using System;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که یک سرویس خارجی (مثلاً ایمیل، پیامک، API دیگر) با مشکل مواجه شده است.
/// </summary>
public sealed class ExternalServiceException : InfrastructureException
{
    /// <summary>
    /// نام سرویس خارجی.
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// ساخت یک استثنا با نام سرویس و پیام.
    /// </summary>
    public ExternalServiceException(string serviceName, string message, Exception? innerException = null)
        : base("external_service_failure", message, innerException ?? new Exception(message))
    {
        ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
    }
}