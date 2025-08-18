// File: AuthSystem.Domain/Common/Specifications/ISpecification.cs
using System;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>
    /// قرارداد Specification
    /// - تعریف predicate قابل ترکیب برای فیلتر/اعتبارسنجی
    /// </summary>
    public interface ISpecification<T>
    {
        /// <summary>عبارت شرطی قابل ترکیب (LINQ Expression)</summary>
        Expression<Func<T, bool>> ToExpression();

        /// <summary>آیا نمونه ورودی شرط را ارضا می‌کند؟</summary>
        bool IsSatisfiedBy(T candidate);
    }
}