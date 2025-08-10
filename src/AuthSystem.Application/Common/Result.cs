using AuthSystem.Domain.Enums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AuthSystem.Application.Common;

/// <summary>
/// نتیجه یک عملیات در لایه Application.
/// این کلاس برای بازگرداندن وضعیت موفقیت یا خطا به لایه Api استفاده می‌شود.
/// </summary>
/// <example>
/// Result.Success() یا Result.Failure(AuthStatus.InvalidCredentials, "ایمیل یا رمز عبور نامعتبر است")
/// </example>
public class Result
{
    /// <summary>
    /// نشان‌دهنده موفقیت عملیات
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// وضعیت احراز هویت (موفقیت، اطلاعات نامعتبر، حساب قفل شده و غیره)
    /// </summary>
    public AuthStatus Status { get; }

    /// <summary>
    /// پیام توضیحاتی برای نمایش به کاربر یا لاگ
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// لیست خطاهای اعتبارسنجی یا عملیاتی (فقط در صورت شکست)
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }

    /// <summary>
    /// سازنده protected — برای کنترل ایجاد نمونه و امکان ارث‌بری
    /// </summary>
    protected Result(bool isSuccess, AuthStatus status, string? message, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Status = status;
        Message = message;
        Errors = errors.ToList().AsReadOnly();
    }

    /// <summary>
    /// ایجاد یک نتیجه موفق
    /// </summary>
    public static Result Success(AuthStatus status = AuthStatus.Success, string? message = null)
        => new Result(true, status, message, Enumerable.Empty<string>());

    /// <summary>
    /// ایجاد یک نتیجه ناموفق
    /// </summary>
    public static Result Failure(AuthStatus status, string? message = null, IEnumerable<string>? errors = null)
        => new Result(false, status, message, errors ?? Enumerable.Empty<string>());

    /// <summary>
    /// ایجاد نتیجه ناموفق از نتیجه اعتبارسنجی FluentValidation
    /// </summary>
    public static Result FromValidationResult(ValidationResult validationResult, AuthStatus status = AuthStatus.InvalidCredentials)
    {
        if (validationResult.IsValid)
            return Success();

        var errors = validationResult.Errors.Select(e => e.ErrorMessage);
        return Failure(status, "داده‌های ورودی نامعتبر هستند", errors);
    }

    /// <summary>
    /// ترکیب چندین نتیجه. اگر هر کدوم از نتایج شکست خورده باشه، نتیجه نهایی هم شکست می‌خوره.
    /// </summary>
    public static Result Combine(params Result[] results)
    {
        var failedResult = results.FirstOrDefault(r => !r.IsSuccess);
        if (failedResult != null)
            return Failure(failedResult.Status, failedResult.Message, failedResult.Errors);

        return Success();
    }
}

/// <summary>
/// نتیجه یک عملیات که شامل داده (مثلاً اطلاعات کاربر) است
/// </summary>
/// <typeparam name="T">نوع داده‌ای که باید بازگردانده شود</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// داده مربوط به نتیجه (مثلاً LoginResponse)
    /// فقط در صورت موفقیت مقدار دارد
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// سازنده protected
    /// </summary>
    protected Result(bool isSuccess, T? value, AuthStatus status, string? message, IEnumerable<string> errors)
        : base(isSuccess, status, message, errors)
    {
        Value = value;
    }

    /// <summary>
    /// ایجاد یک نتیجه موفق با داده
    /// </summary>
    public static Result<T> Succeeded(T value, AuthStatus status = AuthStatus.Success, string? message = null)
        => new Result<T>(true, value, status, message, Enumerable.Empty<string>());

    /// <summary>
    /// ایجاد یک نتیجه ناموفق با داده (معمولاً داده null است)
    /// </summary>
    public static Result<T> Failed(AuthStatus status, string? message = null, IEnumerable<string>? errors = null)
        => new Result<T>(false, default!, status, message, errors ?? Enumerable.Empty<string>());

    /// <summary>
    /// ایجاد نتیجه ناموفق از نتیجه اعتبارسنجی FluentValidation
    /// </summary>
    public static Result<T> FromValidationResult(ValidationResult validationResult, AuthStatus status = AuthStatus.InvalidCredentials)
    {
        if (validationResult.IsValid)
            return Succeeded(default(T)!);

        var errors = validationResult.Errors.Select(e => e.ErrorMessage);
        return Failed(status, "داده‌های ورودی نامعتبر هستند", errors);
    }

    /// <summary>
    /// ترکیب نتایج با داده. اولین شکست را برمی‌گرداند یا موفقیت با مقدار تجمیع‌شده.
    /// </summary>
    public static Result<T> Combine(Func<IEnumerable<T>, T> aggregator, params (Result result, T value)[] resultValues)
    {
        var failedResult = resultValues.Select(r => r.result).FirstOrDefault(r => !r.IsSuccess);
        if (failedResult != null)
            return Failed(failedResult.Status, failedResult.Message, failedResult.Errors);

        var values = resultValues.Select(r => r.value);
        return Succeeded(aggregator(values));
    }
}