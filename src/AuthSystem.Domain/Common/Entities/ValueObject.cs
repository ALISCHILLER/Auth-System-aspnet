// File: AuthSystem.Domain/Common/Entities/ValueObject.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Entities
{
    /// <summary>
    /// کلاس پایه برای تمام Value Objectها
    /// - فاقد هویت مستقل؛ تساوی بر اساس «اجزای برابری» است
    /// - Immutable و Self-Validating در پیاده‌سازی‌های مشتق
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>اجزایی که در مقایسهٔ برابری استفاده می‌شوند</summary>
        protected abstract IEnumerable<object?> GetEqualityComponents();

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType()) return false;
            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // استفاده از HashCode برای ترکیب هش‌کدها
            return GetEqualityComponents()
                .Where(x => x is not null)
                .Select(x => x!.GetHashCode())
                .DefaultIfEmpty(0)
                .Aggregate((x, y) => HashCode.Combine(x, y));
        }

        /// <inheritdoc />
        public bool Equals(ValueObject? other) => Equals((object?)other);

        /// <summary>
        /// ساخت نسخهٔ کپی از VO جهت ایجاد حالت جدید (Immutability-friendly)
        /// </summary>
        protected static T Copy<T>(T source, Action<T> update) where T : ValueObject
        {
            var copy = (T)source.MemberwiseClone();
            update(copy);
            return copy;
        }

        public static bool operator ==(ValueObject? a, ValueObject? b)
            => a is null && b is null || a is not null && a.Equals(b);

        public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

        /// <inheritdoc />
        public override string ToString()
        {
            var components = GetEqualityComponents().Select(c => c?.ToString() ?? "null");
            return $"{GetType().Name}({string.Join(", ", components)})";
        }
    }
}