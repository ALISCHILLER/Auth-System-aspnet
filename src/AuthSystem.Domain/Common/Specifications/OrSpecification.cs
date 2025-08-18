// File: AuthSystem.Domain/Common/Specifications/OrSpecification.cs
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>
    /// Specification منطقی OR با پشتیبانی کامل از EF Core
    /// - از ExpressionComposer برای سازگاری با EF Core استفاده می‌کند
    /// - از Expression.Invoke اجتناب شده است
    /// </summary>
    public sealed class OrSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        /// <summary>
        /// ترکیب عبارت‌های منطقی بدون استفاده از Expression.Invoke
        /// - کاملاً قابل ترجمه توسط EF Core
        /// - استفاده از ExpressionComposer برای جایگزینی پارامترها
        /// </summary>
        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();

            // استفاده از ExpressionComposer برای ترکیب امن عبارت‌ها
            return ExpressionComposer.OrElse(leftExpr, rightExpr);
        }
    }
}