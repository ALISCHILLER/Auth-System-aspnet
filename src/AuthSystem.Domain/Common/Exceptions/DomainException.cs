using System;

namespace AuthSystem.Domain.Common.Exceptions;

/// <summary>
/// کلاس پایه برای استثناهای دامنه
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// کد خطا برای پردازش‌های بعدی
    /// </summary>
    public virtual string ErrorCode => GetType().Name;

    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    protected DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// سازنده با پیام خطا و استثنای داخلی
    /// </summary>
    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}