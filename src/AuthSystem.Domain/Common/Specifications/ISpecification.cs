using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// اینترفیس پایه برای مشخصات
/// </summary>
public interface ISpecification<T>
{
    /// <summary>
    /// عبارت شرطی مشخصات
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// لیست عبارات مرتب‌سازی
    /// </summary>
    List<Expression<Func<T, object>>> OrderBy { get; }

    /// <summary>
    /// لیست عبارات مرتب‌سازی معکوس
    /// </summary>
    List<Expression<Func<T, object>>> OrderByDescending { get; }

    /// <summary>
    /// تعداد عناصر برای پاگینیشن
    /// </summary>
    int? Take { get; }

    /// <summary>
    /// تعداد عناصر برای رد کردن (پاگینیشن)
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// آیا پاگینیشن فعال است
    /// </summary>
    bool IsPagingEnabled { get; }

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات مطابقت دارد
    /// </summary>
    bool IsSatisfiedBy(T entity);
}