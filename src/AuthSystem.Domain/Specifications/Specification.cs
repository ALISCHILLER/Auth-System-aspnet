using System;
using System.Linq;
using System.Linq.Expressions;

namespace AuthSystem.Domain.Specifications;

/// <summary>
/// کلاس پایه برای تمام Specifications
/// این کلاس به ما امکان می‌دهد معیارهای کوئری را به صورت اشیاء تعریف کنیم
/// </summary>
/// <typeparam name="T">نوع موجودیت</typeparam>
public abstract class Specification<T>
{
    /// <summary>
    /// عبارت لامبدا که معیار فیلتر را تعریف می‌کند
    /// </summary>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// تبدیل Specification به عبارت بولی
    /// </summary>
    public Func<T, bool> ToFunc()
    {
        return ToExpression().Compile();
    }

    /// <summary>
    /// بررسی اینکه آیا یک موجودیت معیارهای Specification را دارد یا خیر
    /// </summary>
    /// <param name="entity">موجودیت</param>
    /// <returns>در صورت مطابقت true باز می‌گرداند</returns>
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression();
        return predicate.Compile()(entity);
    }

    /// <summary>
    /// ترکیب دو Specification با عملگر AND
    /// </summary>
    /// <param name="left">Specification سمت چپ</param>
    /// <param name="right">Specification سمت راست</param>
    /// <returns>Specification ترکیبی</returns>
    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
    {
        return new AndSpecification<T>(left, right);
    }

    /// <summary>
    /// ترکیب دو Specification با عملگر OR
    /// </summary>
    /// <param name="left">Specification سمت چپ</param>
    /// <param name="right">Specification سمت راست</param>
    /// <returns>Specification ترکیبی</returns>
    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
    {
        return new OrSpecification<T>(left, right);
    }

    /// <summary>
    /// معکوس کردن یک Specification
    /// </summary>
    /// <param name="specification">Specification مورد نظر</param>
    /// <returns>Specification معکوس</returns>
    public static Specification<T> operator !(Specification<T> specification)
    {
        return new NotSpecification<T>(specification);
    }
}