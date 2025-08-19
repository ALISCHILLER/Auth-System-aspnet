using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// مشخصات منفی
/// </summary>
public class NotSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// مشخصات اصلی
    /// </summary>
    private readonly ISpecification<T> _specification;

    /// <summary>
    /// سازنده با مشخصات
    /// </summary>
    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    /// <summary>
    /// عبارت شرطی مشخصات
    /// </summary>
    public Expression<Func<T, bool>>? Criteria =>
        Expression.Lambda<Func<T, bool>>(
            Expression.Not(_specification.Criteria!.Body),
            _specification.Criteria.Parameters);

    /// <summary>
    /// لیست عبارات مرتب‌سازی
    /// </summary>
    public List<Expression<Func<T, object>>> OrderBy => _specification.OrderBy;

    /// <summary>
    /// لیست عبارات مرتب‌سازی معکوس
    /// </summary>
    public List<Expression<Func<T, object>>> OrderByDescending => _specification.OrderByDescending;

    /// <summary>
    /// تعداد عناصر برای پاگینیشن
    /// </summary>
    public int? Take => _specification.Take;

    /// <summary>
    /// تعداد عناصر برای رد کردن (پاگینیشن)
    /// </summary>
    public int? Skip => _specification.Skip;

    /// <summary>
    /// آیا پاگینیشن فعال است
    /// </summary>
    public bool IsPagingEnabled => _specification.IsPagingEnabled;

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات مطابقت دارد
    /// </summary>
    public bool IsSatisfiedBy(T entity)
    {
        return !_specification.IsSatisfiedBy(entity);
    }
}