using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications;

/// <summary>
/// Specification ترکیبی که نتیجه یک Specification را معکوس می‌کند
/// </summary>
/// <typeparam name="T">نوع موجودیت</typeparam>
public class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    public NotSpecification(Specification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expression, parameter));

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}