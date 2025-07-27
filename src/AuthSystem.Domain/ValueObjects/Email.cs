using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthSystem.Domain.ValueObjects
{
    /// <summary>
    /// Value Object برای نشانی ایمیل
    /// </summary>

    public class Email
    {
        private readonly string _email;

        /// <summary>
        /// سازنده خصوصی برای جلوگیری از ایجاد مستقیم
        /// </summary>
        /// <param name="email">آدرس ایمیل</param>
        private Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("ایمیل نمی‌تواند خالی باشد.", nameof(email));

            email = email.Trim().ToLowerInvariant();

            if (!IsValidEmail(email))
                throw new ArgumentException("فرمت ایمیل نامعتبر است.", nameof(email));

            _email = email;
        }

        /// <summary>
        /// تبدیل implicit به string
        /// </summary>
        public static implicit operator string(Email email) => email._email;

        /// <summary>
        /// تبدیل implicit از string
        /// </summary>
        public static implicit operator Email(string email) => new Email(email);

        /// <summary>
        /// بازگرداندن نمایش رشته‌ای ایمیل
        /// </summary>
        public override string ToString() => _email;

        /// <summary>
        /// مقایسه دو شیء Email
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Email other)
                return _email.Equals(other._email, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        /// <summary>
        /// دریافت HashCode برای Email
        /// </summary>
        public override int GetHashCode() => _email.GetHashCode(StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// اعتبارسنجی فرمت ایمیل با استفاده از Regular Expression
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            // الگوی ساده‌تر برای اعتبارسنجی ایمیل
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
            // برای اعتبارسنجی دقیق‌تر می‌توان از DataAnnotations استفاده کرد:
            // return new EmailAddressAttribute().IsValid(email);
        }
    }
}