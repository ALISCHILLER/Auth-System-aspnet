using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common
{
    /// <summary>
    /// کلاس پایه برای تمام Value Objects در سیستم
    /// Value Objects دارای هویت نیستند و بر اساس ارزششان مقایسه می‌شوند
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// مقایسه عمیق برای برابری دو Value Object
        /// </summary>
        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null ^ right is null)
                return false;

            return left?.Equals(right) != false;
        }

        /// <summary>
        /// مقایسه عمیق برای نابرابری دو Value Object
        /// </summary>
        public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

        /// <summary>
        /// دریافت کامپوننت‌های مورد نیاز برای مقایسه برابری
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// مقایسه برابری عمیق
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// مقایسه برابری نوع‌دار
        /// </summary>
        public bool Equals(ValueObject? other) => Equals((object?)other);

        /// <summary>
        /// محاسبه هش کد بر اساس کامپوننت‌های برابری
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// کپی عمیق از Value Object
        /// </summary>
        public virtual ValueObject Copy()
        {
            return (ValueObject)MemberwiseClone();
        }

        /// <summary>
        /// بررسی برابری با در نظر گرفتن ترتیب
        /// </summary>
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (left is null ^ right is null)
                return false;

            return left?.Equals(right) != false;
        }

        /// <summary>
        /// بررسی نابرابری با در نظر گرفتن ترتیب
        /// </summary>
        protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
        {
            return !EqualOperator(left, right);
        }
    }
}