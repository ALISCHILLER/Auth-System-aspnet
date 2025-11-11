using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// اکستنشن‌های مشخصات
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// ترکیب دو مشخصات با AND
    /// </summary>
    public static ISpecification<T> And<T>(
        this ISpecification<T> first,
        ISpecification<T> second)
    {
        return new AndSpecification<T>(first, second);
    }

    /// <summary>
    /// ترکیب دو مشخصات با OR
    /// </summary>
    public static ISpecification<T> Or<T>(
        this ISpecification<T> first,
        ISpecification<T> second)
    {
        return new OrSpecification<T>(first, second);
    }

    /// <summary>
    /// ترکیب دو مشخصات با NOT
    /// </summary>
    public static ISpecification<T> Not<T>(
        this ISpecification<T> specification)
    {
        return new NotSpecification<T>(specification);
    }

    /// <summary>
    /// اعمال مشخصات بر روی کوئری
    /// </summary>
    public static IQueryable<T> ApplySpecification<T>(
        this IQueryable<T> query,
        ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        foreach (var orderBy in specification.OrderBy)
        {
            query = query.OrderBy(orderBy);
        }

        foreach (var orderByDescending in specification.OrderByDescending)
        {
            query = query.OrderByDescending(orderByDescending);
        }

        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip ?? 0)
                         .Take(specification.Take ?? 0);
        }

        return query;
    }

    /// <summary>
    /// تبدیل مشخصات به عبارت
    /// </summary>
    public static Expression<Func<T, bool>> ToExpression<T>(this ISpecification<T> specification)
    {
        return specification.Criteria ?? (x => true);
    }
}