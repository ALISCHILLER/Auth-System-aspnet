using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common;

/// <summary>
/// کلاس پایه برای تمام Value Objects در سیستم
/// Value Objects دارای هویت نیستند و بر اساس ارزششان مقایسه می‌شوند
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// دریافت کامپوننت‌های مورد نیاز برای مقایسه برابری
    /// هر Value Object باید این متد را پیاده‌سازی کند
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// مقایسه برابری عمیق بر اساس کامپوننت‌ها
    /// </summary>
    public override bool Equals(object? obj)
    {
        // اگر شیء مقایسه‌شونده null باشد یا نوع آن متفاوت باشد
        if (obj == null || obj.GetType() != GetType())
            return false;
            
        // تبدیل به ValueObject و مقایسه کامپوننت‌ها
        var other = (ValueObject)obj;
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// محاسبه هش کد بر اساس کامپوننت‌های برابری
    /// این متد برای استفاده در ساختارهای داده‌ای مانند Dictionary مهم است
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// مقایسه برابری نوع‌دار
    /// </summary>
    public bool Equals(ValueObject? other) => Equals((object?)other);

    /// <summary>
    /// عملگر تساوی برای مقایسه دو Value Object
    /// </summary>
    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        // اگر هر دو null هستند
        if (a is null && b is null)
            return true;
        // اگر یکی null و دیگری نه
        if (a is null || b is null)
            return false;
        // فراخوانی متد Equals
        return a.Equals(b);
    }

    /// <summary>
    /// عملگر عدم تساوی برای مقایسه دو Value Object
    /// </summary>
    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);
}