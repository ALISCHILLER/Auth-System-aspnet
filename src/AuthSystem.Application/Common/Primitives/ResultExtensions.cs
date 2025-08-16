
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// متدهای کمکی برای کار با Result و Result<T>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// تبدیل نتیجه‌های اعتبارسنجی FluentValidation به Result.
    /// </summary>
    public static Result FromValidationResult(this ValidationResult validationResult, Error? error = null)
    {
        if (validationResult.IsValid)
            return Result.Success();

        var errors = validationResult.Errors.Select(f => Error.Validation(
            f.ErrorMessage,
            f.PropertyName,
            $"Validation.{f.PropertyName}.{f.ErrorCode}"));

        return Result.Failure(errors);
    }

    /// <summary>
    /// تبدیل نتیجه‌های اعتبارسنجی FluentValidation به Result<T>.
    /// </summary>
    public static Result<T> FromValidationResult<T>(this ValidationResult validationResult, T value)
    {
        return validationResult.IsValid
            ? Result<T>.Success(value)
            : Result<T>.Failure(validationResult.Errors.Select(f => Error.Validation(
                f.ErrorMessage,
                f.PropertyName,
                $"Validation.{f.PropertyName}.{f.ErrorCode}")));
    }

    /// <summary>
    /// اجرای عملیاتی که Result برمی‌گرداند، فقط در صورت موفقیت نتیجه فعلی.
    /// </summary>
    public static async Task<Result<T>> Then<T>(this Task<Result> task, Func<Task<Result<T>>> next)
    {
        var result = await task;
        return result.IsSuccess ? await next() : Result<T>.Failure(result.Errors);
    }

    /// <summary>
    /// اجرای عملیاتی که Result<T> برمی‌گرداند، فقط در صورت موفقیت نتیجه فعلی.
    /// </summary>
    public static async Task<Result<U>> Then<T, U>(this Task<Result<T>> task, Func<T, Task<Result<U>>> next)
    {
        var result = await task;
        return result.IsSuccess ? await next(result.Value) : Result<U>.Failure(result.Errors);
    }
}