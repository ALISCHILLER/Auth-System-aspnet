namespace AuthSystem.Application.Common.Exceptions;

/// <summary>
/// استثنا برای زمانی که اعتبارسنجی درخواست شکست می‌خورد.
/// شامل مجموعه‌ای از <see cref="Error"/>های ساختاریافته است که می‌توانند به کلاینت یا سیستم لاگ ارسال شوند.
/// </summary>
public sealed class ValidationException : ApplicationException
{
    /// <summary>
    /// مجموعهٔ خطاهای اعتبارسنجی.
    /// اگر در سازنده خطاها مشخص نشده باشند، حداقل یک <see cref="Error"/> از نوع Validation ایجاد می‌شود.
    /// </summary>
    public override IReadOnlyCollection<Error> Errors => _errors ?? new[] { Error.Validation(Message) };

    private readonly IReadOnlyCollection<Error>? _errors;

    /// <summary>
    /// ساخت یک استثنا با مجموعه‌ای از <see cref="Error"/>های اعتبارسنجی.
    /// </summary>
    public ValidationException(IEnumerable<Error> errors)
        : base("validation_error", "خطاهای اعتبارسنجی رخ داده است.")
    {
        _errors = (errors ?? throw new ArgumentNullException(nameof(errors))).ToList().AsReadOnly();
    }

    /// <summary>
    /// ساخت یک استثنا با یک پیام اعتبارسنجی.
    /// </summary>
    public ValidationException(string message, string? propertyName = null)
        : base("validation_error", message, new[] { Error.Validation(message, propertyName) })
    {
    }
}