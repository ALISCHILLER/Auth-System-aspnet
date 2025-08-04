using AuthSystem.Domain.Common;
using AuthSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects
{
    /// <summary>
    /// Value Object رمز عبور
    /// مسئول اعتبارسنجی و مدیریت رمز عبور
    /// </summary>
    public class Password : ValueObject
    {
        /// <summary>
        /// مقدار رمز عبور به صورت هش شده یا متنی (بسته به طراحی)
        /// </summary>
        public string Value { get; private set; }

        // سازنده خصوصی
        private Password(string value)
        {
            Value = value;
        }

        /// <summary>
        /// ساخت یک نمونه معتبر از Password با اعتبارسنجی
        /// </summary>
        public static Password Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");

            if (!IsValidPassword(password))
                throw InvalidPasswordException.ForInvalidPassword();

            // اینجا می‌تونی هش کردن رمز رو هم انجام بدی اگر لازم هست
            return new Password(password);
        }

        // اعتبارسنجی رمز عبور (حداقل 8 کاراکتر، شامل حرف بزرگ، حرف کوچک، عدد، و نماد)
        private static bool IsValidPassword(string password)
        {
            var hasMinimum8Chars = password.Length >= 8;
            var hasUpperChar = Regex.IsMatch(password, @"[A-Z]");
            var hasLowerChar = Regex.IsMatch(password, @"[a-z]");
            var hasNumber = Regex.IsMatch(password, @"\d");
            var hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

            return hasMinimum8Chars && hasUpperChar && hasLowerChar && hasNumber && hasSpecialChar;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}