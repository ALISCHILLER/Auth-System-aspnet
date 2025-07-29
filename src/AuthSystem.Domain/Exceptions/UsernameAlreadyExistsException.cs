using System;

namespace AuthSystem.Domain.Exceptions;

/// <summary>
/// استثنا برای زمانی که نام کاربری تکراری است
/// </summary>
public class UsernameAlreadyExistsException : DomainException
{
    /// <summary>
    /// نام کاربری که تکراری است
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// سازنده با نام کاربری
    /// </summary>
    /// <param name="username">نام کاربری</param>
    public UsernameAlreadyExistsException(string username)
        : base($"نام کاربری {username} قبلاً ثبت شده است")
    {
        Username = username;
    }

    /// <summary>
    /// سازنده با نام کاربری و پیام خطا
    /// </summary>
    /// <param name="username">نام کاربری</param>
    /// <param name="message">پیام خطا</param>
    public UsernameAlreadyExistsException(string username, string message)
        : base(message)
    {
        Username = username;
    }
}