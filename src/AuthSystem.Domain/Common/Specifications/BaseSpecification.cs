using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// کلاس پایه برای مشخصات
/// </summary>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// عبارت شرطی مشخصات
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// لیست عبارات مرتب‌سازی
    /// </summary>
    public List<Expression<Func<T, object>>> OrderBy { get; } = new List<Expression<Func<T, object>>>();

    /// <summary>
    /// لیست عبارات مرتب‌سازی معکوس
    /// </summary>
    public List<Expression<Func<T, object>>> OrderByDescending { get; } = new List<Expression<Func<T, object>>>();

    /// <summary>
    /// تعداد عناصر برای پاگینیشن
    /// </summary>
    public int? Take { get; private set; }

    /// <summary>
    /// تعداد عناصر برای رد کردن (پاگینیشن)
    /// </summary>
    public int? Skip { get; private set; }

    /// <summary>
    /// آیا پاگینیشن فعال است
    /// </summary>
    public bool IsPagingEnabled { get; private set; }

    /// <summary>
    /// سازنده با عبارت شرطی
    /// </summary>
    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// سازنده پیش‌فرض
    /// </summary>
    protected BaseSpecification()
    {
    }

    /// <summary>
    /// تنظیم مرتب‌سازی
    /// </summary>
    protected virtual void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy.Add(orderByExpression);
    }

    /// <summary>
    /// تنظیم مرتب‌سازی معکوس
    /// </summary>
    protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending.Add(orderByDescendingExpression);
    }

    /// <summary>
    /// تنظیم پاگینیشن
    /// </summary>
    protected virtual void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات مطابقت دارد
    /// </summary>
    public virtual bool IsSatisfiedBy(T entity)
    {
        if (Criteria == null)
            return true;

        return Criteria.Compile()(entity);
    }
}