using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// مشخصات ترکیبی AND
/// </summary>
public class AndSpecification<T> : CompositeSpecification<T>
{
    /// <summary>
    /// سازنده با دو مشخصات
    /// </summary>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
       : base(left, right)
    {
    }

    /// <summary>
    /// بررسی آیا شیء مورد نظر با مشخصات ترکیبی مطابقت دارد
    /// </summary>
    public override bool IsSatisfiedBy(T entity)
    {
        return Left.IsSatisfiedBy(entity) && Right.IsSatisfiedBy(entity);
    }
    /// <summary>
    /// ترکیب عبارات معیار برای مشخصات AND
    /// </summary>
    protected override Expression<Func<T, bool>> CombineCriteria(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return ExpressionComposer.And(left, right);
    }
}