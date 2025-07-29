using System;

namespace AuthSystem.Domain.Exceptions
{
    /// <summary>
    /// استثنا برای زمانی که اعتبارسنجی ناموفق بوده است
    /// </summary>
    public class InvalidCredentialsException : DomainException
    {
        /// <summary>
        /// نام کاربری یا ایمیل ورودی
        /// </summary>
        public string? UsernameOrEmail { get; }

        /// <summary>
        /// سازنده پیش‌فرض
        /// </summary>
        public InvalidCredentialsException()
            : base("نام کاربری یا رمز عبور نادرست است") { }

        /// <summary>
        /// سازنده با نام کاربری یا ایمیل
        /// </summary>
        /// <param name="usernameOrEmail">نام کاربری یا ایمیل</param>
        public InvalidCredentialsException(string usernameOrEmail)
            : base("نام کاربری یا رمز عبور نادرست است")
        {
            UsernameOrEmail = usernameOrEmail;
        }

        /// <summary>
        /// سازنده با پیام خطا سفارشی
        /// </summary>
        /// <param name="message">پیام خطا</param>
        /// <param name="isCustomMessage">برای تمایز سازنده</param>
        public InvalidCredentialsException(string message, bool isCustomMessage)
            : base(message) { }
    }
}
