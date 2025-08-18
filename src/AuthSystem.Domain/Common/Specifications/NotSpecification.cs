// File: AuthSystem.Domain/Common/Specifications/NotSpecification.cs
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>Specification منطقی NOT</summary>
    public sealed class NotSpecification<T> : BaseSpecification<T>
    {
        private readonly BaseSpecification<T> _spec;

        public NotSpecification(BaseSpecification<T> spec)
        {
            _spec = spec;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expr = _spec.ToExpression();
            var param = expr.Parameters[0];
            var body = Expression.Not(expr.Body);
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}