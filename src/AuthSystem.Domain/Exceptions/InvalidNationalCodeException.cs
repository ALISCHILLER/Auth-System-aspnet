using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که کد ملی نامعتبر است
/// </summary>
public class InvalidNationalCodeException : DomainException
{
    /// <summary>
    /// کد ملی که باعث بروز خطا شده
    /// </summary>
    public string InvalidCode { get; }
    
    /// <summary>
    /// سازنده پیش‌فرض
    /// </summary>
    public InvalidNationalCodeException() 
        : base("کد ملی وارد شده نامعتبر است") 
    {
        InvalidCode = string.Empty;
    }
    
    /// <summary>
    /// سازنده با پیام خطا
    /// </summary>
    /// <param name="message">پیام خطا</param>
    public InvalidNationalCodeException(string message) 
        : base(message)
    {
        InvalidCode = string.Empty;
    }
    
    /// <summary>
    /// سازنده با کد ملی نامعتبر و پیام خطا
    /// </summary>
    /// <param name="invalidCode">کد ملی نامعتبر</param>
    /// <param name="message">پیام خطا</param>
    public InvalidNationalCodeException(string invalidCode, string message) 
        : base(message)
    {
        InvalidCode = invalidCode;
    }
    
    /// <summary>
    /// سازنده با کد ملی نامعتبر، پیام خطا و استثنا داخلی
    /// </summary>
    /// <param name="invalidCode">کد ملی نامعتبر</param>
    /// <param name="message">پیام خطا</param>
    /// <param name="innerException">استثنا داخلی</param>
    public InvalidNationalCodeException(string invalidCode, string message, Exception innerException) 
        : base(message, innerException)
    {
        InvalidCode = invalidCode;
    }
}