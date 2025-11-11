using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای یافتن نشدن کاربر
/// این استثنا زمانی رخ می‌دهد که سیستم نتواند کاربر مورد نظر را پیدا کند
/// </summary>
public class UserNotFoundException : DomainException
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public Guid? UserId { get; }

    /// <summary>
    /// نام کاربری
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// آدرس ایمیل
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public override string ErrorCode => "UserNotFound";

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    public UserNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// سازنده با شناسه کاربر و پیام خطا
    /// </summary>
    public UserNotFoundException(Guid userId, string message)
        : this(message)
    {
        UserId = userId;
    }

    /// <summary>
    /// سازنده با نام کاربری و پیام خطا
    /// </summary>
    public UserNotFoundException(string username, string message)
        : this(message)
    {
        Username = username;
    }

    /// <summary>
    /// سازنده با آدرس ایمیل و پیام خطا
    /// </summary>
    public UserNotFoundException(string email, string message, bool isEmail)
        : this(message)
    {
        Email = email;
    }

    /// <summary>
    /// ایجاد استثنا برای یافتن نشدن کاربر با شناسه
    /// </summary>
    public static UserNotFoundException ForId(Guid userId)
    {
        return new UserNotFoundException(userId, $"کاربر با شناسه {userId} یافت نشد");
    }

    /// <summary>
    /// ایجاد استثنا برای یافتن نشدن کاربر با نام کاربری
    /// </summary>
    public static UserNotFoundException ForUsername(string username)
    {
        return new UserNotFoundException(username, $"کاربر با نام کاربری '{username}' یافت نشد");
    }

    /// <summary>
    /// ایجاد استثنا برای یافتن نشدن کاربر با آدرس ایمیل
    /// </summary>
    public static UserNotFoundException ForEmail(string email)
    {
        return new UserNotFoundException(email, $"کاربر با آدرس ایمیل '{email}' یافت نشد", true);
    }

    /// <summary>
    /// ایجاد استثنا برای یافتن نشدن کاربر با شماره تلفن
    /// </summary>
    public static UserNotFoundException ForPhoneNumber(string phoneNumber)
    {
        return new UserNotFoundException($"کاربر با شماره تلفن '{phoneNumber}' یافت نشد");
    }

    /// <summary>
    /// ایجاد استثنا برای یافتن نشدن کاربر با توکن تأیید
    /// </summary>
    public static UserNotFoundException ForVerificationToken(string token)
    {
        return new UserNotFoundException($"کاربر مربوط به توکن تأیید '{token}' یافت نشد");
    }
}