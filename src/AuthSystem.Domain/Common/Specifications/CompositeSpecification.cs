using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// مشخصات ترکیبی
/// </summary>
public abstract class CompositeSpecification<T> : ISpecification<T>
{

    /// <summary>
    /// سازنده با دو مشخصات
    /// </summary>
    protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }

    /// <summary>
    /// مشخصات سمت چپ
    /// </summary>
    public ISpecification<T> Left { get; }

    /// <summary>
    /// مشخصات سمت راست
    /// </summary>
    public ISpecification<T> Right { get; }

    /// <summary>
    /// عبارت شرطی ترکیب شده
    /// </summary>
    public virtual Expression<Func<T, bool>>? Criteria
    {
        get
        {
            if (Left.Criteria is null)
            {
                return Right.Criteria;
            }

            if (Right.Criteria is null)
            {
                return Left.Criteria;
            }

            return CombineCriteria(Left.Criteria, Right.Criteria);
        }
    }

    /// <summary>
    /// لیست عبارات مرتب‌سازی
    /// </summary>
    public virtual List<Expression<Func<T, object>>> OrderBy => MergeOrderings(spec => spec.OrderBy);

    /// <summary>
    /// لیست عبارات مرتب‌سازی معکوس
    /// </summary>
    public virtual List<Expression<Func<T, object>>> OrderByDescending => MergeOrderings(spec => spec.OrderByDescending);

    /// <summary>
    /// تعداد عناصر برای پاگینیشن
    /// </summary>
    public virtual int? Take => Left.Take ?? Right.Take;

    /// <summary>
    /// تعداد عناصر برای رد کردن (پاگینیشن)
    /// </summary>
    public virtual int? Skip => Left.Skip ?? Right.Skip;

    /// <summary>
    /// آیا پاگینیشن فعال است
    /// </summary>
    public virtual bool IsPagingEnabled => Left.IsPagingEnabled || Right.IsPagingEnabled;

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات ترکیبی مطابقت دارد
    /// </summary>
    public abstract bool IsSatisfiedBy(T entity);

    /// <summary>
    /// نحوه ترکیب عبارات شرطی
    /// </summary>
    protected abstract Expression<Func<T, bool>> CombineCriteria(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right);

    private List<Expression<Func<T, object>>> MergeOrderings(
        Func<ISpecification<T>, List<Expression<Func<T, object>>>> selector)
    {
        return selector(Left)
               .Concat(selector(Right))
               .ToList();
    }
}