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
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// مقایسه برابری عمیق بر اساس کامپوننت‌ها
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// محاسبه هش کود بر اساس کامپوننت‌های برابری
    /// این متد برای استفاده در ساختارهای داده‌ای مانند Dictionary مهم است
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Where(x => x != null)
            .Select(x => x!.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// مقایسه برابری نوع‌دار
    /// </summary>
    public bool Equals(ValueObject? other) => Equals((object?)other);

    /// <summary>
    /// کپی Value Object با تغییر یک یا چند ویژگی (Immutability)
    /// </summary>
    protected static T Copy<T>(T source, Action<T> update) where T : ValueObject
    {
        var copy = (T)source.MemberwiseClone();
        update(copy);
        return copy;
    }

    /// <summary>
    /// عملگر تساوی برای مقایسه دو Value Object
    /// </summary>
    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }

    /// <summary>
    /// عملگر عدم تساوی برای مقایسه دو Value Object
    /// </summary>
    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    /// <summary>
    /// تبدیل به رشته قابل خواندن
    /// </summary>
    public override string ToString()
    {
        var components = GetEqualityComponents()
            .Select(c => c?.ToString() ?? "null");
        return $"{GetType().Name}({string.Join(", ", components)})";
    }
}
