using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects
{
    /// <summary>
    /// Value Object آدرس ایمیل
    /// این کلاس مسئول اعتبارسنجی و مدیریت آدرس ایمیل است
    /// </summary>
    public class Email : ValueObject
    {
        /// <summary>
        /// مقدار آدرس ایمیل
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// سازنده خصوصی برای ایجاد نمونه از طریق متد Create
        /// </summary>
        private Email(string value)
        {
            Value = value;
        }

        /// <summary>
        /// متد استاتیک برای ایجاد نمونه معتبر از ایمیل
        /// </summary>
        /// <param name="value">آدرس ایمیل</param>
        /// <returns>شیء Email معتبر</returns>
        /// <exception cref="System.ArgumentNullException">اگر مقدار ورودی null یا خالی باشد</exception>
        /// <exception cref="InvalidEmailException">اگر فرمت ایمیل نامعتبر باشد</exception>
        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new System.ArgumentNullException(nameof(value), "آدرس ایمیل نمی‌تواند خالی باشد");

            if (!IsValidEmail(value))
                throw InvalidEmailException.ForInvalidEmail(value);

            return new Email(value);
        }

        /// <summary>
        /// اعتبارسنجی فرمت ایمیل با استفاده از Regex
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// کامپوننت‌های مقایسه برای برابری Value Object
        /// ایمیل بدون حساسیت به حروف بزرگ و کوچک مقایسه می‌شود
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }

        /// <summary>
        /// تبدیل به رشته
        /// </summary>
        public override string ToString() => Value;
    }
}
