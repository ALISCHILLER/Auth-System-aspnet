using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای تکراری بودن آدرس ایمیل
/// این استثنا زمانی رخ می‌دهد که سعی شود از آدرس ایمیلی استفاده شود که قبلاً در سیستم وجود دارد
/// </summary>
public class DuplicateEmailException : DomainException
{
    /// <summary>
    /// آدرس ایمیل تکراری
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "DuplicateEmail";

    /// <summary>
    /// سازنده با پیام خطا و آدرس ایمیل
    /// </summary>
    public DuplicateEmailException(string email)
        : base($"آدرس ایمیل '{email}' قبلاً ثبت شده است")
    {
        Email = email;
    }

    /// <summary>
    /// سازنده با پیام خطا، آدرس ایمیل و استثنای داخلی
    /// </summary>
    public DuplicateEmailException(string email, Exception innerException)
        : base($"آدرس ایمیل '{email}' قبلاً ثبت شده است", innerException)
    {
        Email = email;
    }

    /// <summary>
    /// ایجاد استثنا برای آدرس ایمیل تکراری
    /// </summary>
    public static DuplicateEmailException ForEmail(string email)
    {
        return new DuplicateEmailException(email);
    }
}