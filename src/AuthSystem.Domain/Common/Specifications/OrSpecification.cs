using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// مشخصات ترکیبی OR
/// </summary>
public class OrSpecification<T> : CompositeSpecification<T>
{
    /// <summary>
    /// سازنده با دو مشخصات
    /// </summary>
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        : base(left, right)
    {
    }

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات ترکیبی مطابقت دارد
    /// </summary>
    public override bool IsSatisfiedBy(T entity)
    {
        return Left.IsSatisfiedBy(entity) || Right.IsSatisfiedBy(entity);
    }
    /// <summary>
    /// ترکیب عبارات معیار برای مشخصات OR
    /// </summary>
    protected override Expression<Func<T, bool>> CombineCriteria(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return ExpressionComposer.Or(left, right);
    }
}