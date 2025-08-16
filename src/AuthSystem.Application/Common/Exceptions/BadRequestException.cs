using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که درخواست ارسالی نامعتبر است.
/// معمولاً به کد وضعیت HTTP 400 (Bad Request) نگاشت می‌شود.
/// </summary>
public sealed class BadRequestException : ApplicationException
{
    /// <summary>
    /// ساخت یک استثنا با پیام مشخص.
    /// </summary>
    public BadRequestException(string message)
        : base("bad_request", message, new[] { Error.BadRequest(message) })
    {
    }
}