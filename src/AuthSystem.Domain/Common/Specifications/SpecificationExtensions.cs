// File: AuthSystem.Domain/Common/Specifications/SpecificationExtensions.cs
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Specifications
{
    /// <summary>
    /// توابع کمکی برای اعمال Specification روی LINQ/IQueryable
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>اعمال Specification روی یک IQueryable</summary>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> spec)
            => query.Where(spec.ToExpression());

        /// <summary>اعمال Specification روی یک IEnumerable</summary>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ISpecification<T> spec)
            => source.Where(spec.ToExpression().Compile());
    }
}