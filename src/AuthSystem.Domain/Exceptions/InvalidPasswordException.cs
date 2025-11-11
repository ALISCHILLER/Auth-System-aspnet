using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای رمز عبور نامعتبر
/// این استثنا زمانی رخ می‌دهد که رمز عبور از قوانین امنیتی پیروی نکند
/// </summary>
public class InvalidPasswordException : DomainException
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "InvalidPassword";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public InvalidPasswordException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public InvalidPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور خالی
    /// </summary>
    public static InvalidPasswordException ForEmptyPassword()
    {
        return new InvalidPasswordException("رمز عبور نمی‌تواند خالی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور کوتاه
    /// </summary>
    public static InvalidPasswordException ForShortPassword(int minLength)
    {
        return new InvalidPasswordException($"رمز عبور باید حداقل {minLength} کاراکتر باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور فاقد حروف بزرگ
    /// </summary>
    public static InvalidPasswordException ForMissingUppercase()
    {
        return new InvalidPasswordException("رمز عبور باید حداقل یک حرف بزرگ داشته باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور فاقد حروف کوچک
    /// </summary>
    public static InvalidPasswordException ForMissingLowercase()
    {
        return new InvalidPasswordException("رمز عبور باید حداقل یک حرف کوچک داشته باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور فاقد اعداد
    /// </summary>
    public static InvalidPasswordException ForMissingNumbers()
    {
        return new InvalidPasswordException("رمز عبور باید حداقل یک عدد داشته باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور فاقد کاراکترهای خاص
    /// </summary>
    public static InvalidPasswordException ForMissingSpecialCharacters()
    {
        return new InvalidPasswordException("رمز عبور باید حداقل یک کاراکتر خاص داشته باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور مشابه اطلاعات شخصی
    /// </summary>
    public static InvalidPasswordException ForSimilarToPersonalInfo()
    {
        return new InvalidPasswordException("رمز عبور نمی‌تواند مشابه اطلاعات شخصی باشد");
    }

    /// <summary>
    /// ایجاد استثنا برای رمز عبور تکراری
    /// </summary>
    public static InvalidPasswordException ForDuplicatePassword()
    {
        return new InvalidPasswordException("رمز عبور جدید نمی‌تواند مشابه رمز عبور قبلی باشد");
    }
}