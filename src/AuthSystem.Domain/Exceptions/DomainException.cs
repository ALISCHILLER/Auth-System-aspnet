using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا پایه برای تمام استثناهای دامنه
/// تمام استثناهای مربوط به منطق دامنه باید از این کلاس ارث‌بری کنند
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    /// <param name="message">پیام خطا</param>
    protected DomainException(string message) : base(message) { }

    /// <summary>
    /// سازنده با پیام خطا و استثنا داخلی
    /// </summary>
    /// <param name="message">پیام خطا</param>
    /// <param name="innerException">استثنا داخلی</param>
    protected DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}