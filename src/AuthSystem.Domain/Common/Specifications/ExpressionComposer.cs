// File: AuthSystem.Domain/Common/Specifications/ExpressionComposer.cs
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>
    /// ابزار ترکیب امن Expressionها برای EF Core (بدون Expression.Invoke).
    /// Left و Right را روی یک پارامتر مشترک Rebind می‌کند.
    /// - کاملاً قابل ترجمه توسط EF Core
    /// - جایگزین امن برای Expression.Invoke
    /// </summary>
    internal static class ExpressionComposer
    {
        private sealed class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _source;
            private readonly ParameterExpression _target;

            public ParameterReplacer(ParameterExpression source, ParameterExpression target)
            {
                _source = source;
                _target = target;
            }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _source ? _target : base.VisitParameter(node);
        }

        /// <summary>
        /// ترکیب دو عبارت با AND
        /// </summary>
        public static Expression<Func<T, bool>> AndAlso<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            var param = left.Parameters[0];
            var replacedRight = new ParameterReplacer(right.Parameters[0], param).Visit(right.Body)!;
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, replacedRight), param);
        }

        /// <summary>
        /// ترکیب دو عبارت با OR
        /// </summary>
        public static Expression<Func<T, bool>> OrElse<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            var param = left.Parameters[0];
            var replacedRight = new ParameterReplacer(right.Parameters[0], param).Visit(right.Body)!;
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, replacedRight), param);
        }

        /// <summary>
        /// معکوس کردن یک عبارت
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var param = expr.Parameters[0];
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), param);
        }
    }
}