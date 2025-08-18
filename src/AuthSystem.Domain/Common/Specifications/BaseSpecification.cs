// File: AuthSystem.Domain/Common/Specifications/BaseSpecification.cs
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>
    /// پیاده‌سازی پایه Specification با پشتیبانی از AND/OR/NOT
    /// </summary>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T candidate)
            => ToExpression().Compile().Invoke(candidate);

        public BaseSpecification<T> And(BaseSpecification<T> other)
            => new AndSpecification<T>(this, other);

        public BaseSpecification<T> Or(BaseSpecification<T> other)
            => new OrSpecification<T>(this, other);

        public BaseSpecification<T> Not()
            => new NotSpecification<T>(this);
    }
}