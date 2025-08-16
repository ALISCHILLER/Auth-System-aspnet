using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// نوع خطا را برای دسته‌بندی و تصمیم‌گیری‌های بعدی مشخص می‌کند.
/// این نوع به راحتی به کد وضعیت HTTP نگاشت می‌شود.
/// </summary>
public enum ErrorType
{
    Failure,      // خطای عمومی (500)
    Validation,   // خطای اعتبارسنجی (400)
    NotFound,     // پیدا نشد (404)
    Conflict,     // تداخل (409)
    Unauthorized, // عدم احراز هویت (401)
    Forbidden,    // عدم دسترسی (403)
    BadRequest     // عدم دسترسی (403)
}

/// <summary>
/// نمایانگر یک خطای مشخص در دامنه یا اپلیکیشن است.
/// این کلاس غیرقابل تغییر (Immutable) است و تمام اطلاعات لازم برای گزارش، لاگ و ترجمه خطا را دارد.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// یک کد منحصر به فرد برای شناسایی نوع خطا.
    /// مثال: "Users.EmailAlreadyInUse"
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// پیام توصیفی و قابل فهم برای خطا.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// نوع دسته‌بندی خطا برای نگاشت به HTTP Status Code.
    /// </summary>
    public ErrorType Type { get; init; }

    /// <summary>
    /// توضیحات بیشتر (اختیاری، برای توسعه‌دهندگان).
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// داده‌های اضافی مانند نام پراپرتی در ولیدیشن یا context-specific info.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; init; }

    /// <summary>
    /// کلید رشته‌ای که برای lookup در منابع ترجمه (Localization) استفاده می‌شود.
    /// </summary>
    public string? LocalizedMessageKey { get; init; }

    /// <summary>
    /// سازنده خصوصی برای اطمینان از ساخت خطا فقط از طریق متد فکتوری.
    /// </summary>
    private Error(
        string code,
        string message,
        ErrorType type,
        string? description = null,
        IReadOnlyDictionary<string, object>? metadata = null,
        string? localizedMessageKey = null)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Type = type;
        Description = description;
        Metadata = metadata is null
            ? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>())
            : new ReadOnlyDictionary<string, object>(new Dictionary<string, object>(metadata));
        LocalizedMessageKey = localizedMessageKey;
    }

    /// <summary>
    /// متد فکتوری برای ساخت یک خطای جدید.
    /// </summary>
    public static Error Create(
        string code,
        string message,
        ErrorType type = ErrorType.Failure,
        string? description = null,
        IReadOnlyDictionary<string, object>? metadata = null,
        string? localizedMessageKey = null) =>
        new(code, message, type, description, metadata, localizedMessageKey);

    #region Factory Methods پیش‌فرض

    /// <summary>
    /// ساخت خطای اعتبارسنجی.
    /// </summary>
    public static Error Validation(string message = "خطای اعتبارسنجی", string? propertyName = null, string? localizedMessageKey = "error.validation")
    {
        var meta = propertyName is null ? null : new Dictionary<string, object> { ["property"] = propertyName };
        return Create("validation", message, ErrorType.Validation, null, meta, localizedMessageKey);
    }

    /// <summary>
    /// ساخت خطای یافت نشدن.
    /// </summary>
    public static Error NotFound(string message = "منبع یافت نشد", string code = "not_found", string? localizedMessageKey = "error.not_found")
        => Create(code, message, ErrorType.NotFound, null, null, localizedMessageKey);

    /// <summary>
    /// ساخت خطای عدم احراز هویت.
    /// </summary>
    public static Error Unauthorized(string message = "دسترسی غیرمجاز — احراز هویت نشده", string code = "unauthorized", string? localizedMessageKey = "error.unauthorized")
        => Create(code, message, ErrorType.Unauthorized, null, null, localizedMessageKey);

    /// <summary>
    /// ساخت خطای عدم دسترسی.
    /// </summary>
    public static Error Forbidden(string message = "دسترسی غیرمجاز — دسترسی لازم را ندارید", string code = "forbidden", string? localizedMessageKey = "error.forbidden")
        => Create(code, message, ErrorType.Forbidden, null, null, localizedMessageKey);

    /// <summary>
    /// ساخت خطای تداخل (Conflict).
    /// </summary>
    public static Error Conflict(string message = "تداخل داده‌ها", string code = "conflict", string? localizedMessageKey = "error.conflict")
        => Create(code, message, ErrorType.Conflict, null, null, localizedMessageKey);

    /// <summary>
    /// ساخت خطای درخواست نامعتبر.
    /// </summary>
    public static Error BadRequest(string message = "درخواست نامعتبر", string code = "bad_request", string? localizedMessageKey = "error.bad_request")
        => Create(code, message, ErrorType.BadRequest, null, null, localizedMessageKey);

    /// <summary>
    /// ساخت خطای عمومی.
    /// </summary>
    public static Error Failure(string message = "خطای داخلی سیستم", string code = "failure", string? localizedMessageKey = "error.failure")
        => Create(code, message, ErrorType.Failure, null, null, localizedMessageKey);

    #endregion

    /// <summary>
    /// بازگرداندن یک کپی از Error با افزودن/بروزرسانی Metadata (الگوی immutable).
    /// </summary>
    public Error WithMetadata(string key, object value)
    {
        var dictionary = new Dictionary<string, object>(Metadata);
        dictionary[key] = value;
        return this with { Metadata = new ReadOnlyDictionary<string, object>(dictionary) };
    }

    /// <summary>
    /// تبدیل Error به دیکشنری جهت سریالیزه کردن در API Response یا logging.
    /// </summary>
    public IDictionary<string, object> ToDictionary()
    {
        var d = new Dictionary<string, object>
        {
            ["code"] = Code,
            ["message"] = Message,
            ["type"] = Type.ToString()
        };

        if (!string.IsNullOrWhiteSpace(Description)) d["description"] = Description!;
        if (Metadata != null && Metadata.Count > 0) d["metadata"] = Metadata;
        if (!string.IsNullOrWhiteSpace(LocalizedMessageKey)) d["localizedKey"] = LocalizedMessageKey!;

        return d;
    }

    public override string ToString() => $"{Code}: {Message}";
}