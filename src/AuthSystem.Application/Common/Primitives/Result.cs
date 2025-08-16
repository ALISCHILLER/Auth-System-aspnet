// AuthSystem.Application/Common/Primitives/Result.cs

using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// نمایانگر نتیجه یک عملیات است که می‌تواند موفق یا ناموفق باشد.
/// این نسخه برای عملیات‌هایی است که مقداری برنمی‌گردانند.
/// </summary>
public class Result
{
    /// <summary>
    /// آیا عملیات موفقیت‌آمیز بوده است؟
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// آیا عملیات ناموفق بوده است؟
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// لیست خطاهای رخ داده در صورت ناموفق بودن عملیات.
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; }

    /// <summary>
    /// سازنده محافظت‌شده برای جلوگیری از ساخت مستقیم و تشویق به استفاده از متدهای فکتوری.
    /// </summary>
    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// ایجاد یک نتیجه موفق.
    /// </summary>
    public static Result Success() => new(true, new[] { Error.Create("Success", "عملیات با موفقیت انجام شد.", ErrorType.Failure) });

    /// <summary>
    /// ایجاد یک نتیجه ناموفق با یک خطای مشخص.
    /// </summary>
    public static Result Failure(Error error) => new(false, new[] { error });

    /// <summary>
    /// ایجاد یک نتیجه ناموفق با مجموعه‌ای از خطاها.
    /// </summary>
    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);

    /// <summary>
    /// ایجاد یک نتیجه بر اساس شرط.
    /// </summary>
    public static Result Create(bool condition, Error error) =>
        condition ? Success() : Failure(error);

    /// <summary>
    /// ترکیب چندین نتیجه. اگر هر کدام شکست خورده باشد، نتیجه نهایی شکست می‌خورد.
    /// </summary>
    public static Result Combine(params Result[] results)
    {
        var firstFailure = results.FirstOrDefault(r => r.IsFailure);
        return firstFailure ?? Success();
    }
}