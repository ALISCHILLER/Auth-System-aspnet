using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// کلاس برای ترکیب عبارات
/// </summary>
public static class ExpressionComposer
{
    /// <summary>
    /// ترکیب عبارات با AND
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.AndAlso);
    }

    /// <summary>
    /// ترکیب عبارات با OR
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.OrElse);
    }

    /// <summary>
    /// ترکیب عبارات با عملگر مشخص
    /// </summary>
    public static Expression<T> Compose<T>(
        this Expression<T> first,
        Expression<T> second,
        Func<Expression, Expression, Expression> merge)
    {
        var parameter = Expression.Parameter(typeof(T).GetGenericArguments()[0]);

        var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
        var left = leftVisitor.Visit(first.Body);

        var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
        var right = rightVisitor.Visit(second.Body);

        return Expression.Lambda<T>(merge(left, right), parameter);
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;

            return base.Visit(node);
        }
    }
}