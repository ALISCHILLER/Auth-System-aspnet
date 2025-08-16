using System;
using System.Collections.Generic;

namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// نسخه جنریک Result که یک مقدار را در صورت موفقیت با خود حمل می‌کند.
/// </summary>
/// <typeparam name="TValue">نوع مقدار بازگشتی.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// مقدار بازگشتی در صورت موفقیت.
    /// در صورت شکست، دسترسی به این پراپرتی یک استثنا پرتاب می‌کند.
    /// </summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("مقدار نتیجه ناموفق قابل دسترسی نیست.");

    /// <summary>
    /// سازنده محافظت‌شده.
    /// </summary>
    protected Result(bool isSuccess, TValue? value, IEnumerable<Error> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    /// <summary>
    /// ایجاد یک نتیجه موفق با مقدار مشخص.
    /// </summary>
    public static Result<TValue> Success(TValue value) => new(true, value, new[] { Error.Create("Success", "عملیات با موفقیت انجام شد.", ErrorType.Failure) });

    /// <summary>
    /// ایجاد یک نتیجه ناموفق با یک خطای مشخص.
    /// </summary>
    public new static Result<TValue> Failure(Error error) => new(false, default, new[] { error });

    /// <summary>
    /// ایجاد یک نتیجه ناموفق با مجموعه‌ای از خطاها.
    /// </summary>
    public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(false, default, errors);

    /// <summary>
    /// تبدیل ضمنی از مقدار به نتیجه موفق.
    /// این کار باعث می‌شود بتوانید مستقیماً <c>return user;</c> را به جای <c>return Result.Success(user);</c> بنویسید.
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    /// <summary>
    /// تبدیل ضمنی از خطا به نتیجه ناموفق.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}