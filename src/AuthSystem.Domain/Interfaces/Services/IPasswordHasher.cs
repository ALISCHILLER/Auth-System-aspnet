namespace AuthSystem.Domain.Interfaces.Services;

/// <summary>
/// رابط برای سرویس هش‌کردن رمز عبور
/// این رابط روش‌های لازم برای هش کردن و تأیید رمز عبور را تعریف می‌کند
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// هش کردن یک رمز عبور
    /// </summary>
    /// <param name="password">رمز عبور متن ساده</param>
    /// <returns>هش رمز عبور</returns>
    string HashPassword(string password);

    /// <summary>
    /// تأیید یک رمز عبور با هش آن
    /// </summary>
    /// <param name="hashedPassword">هش رمز عبور</param>
    /// <param name="providedPassword">رمز عبور ارائه شده</param>
    /// <returns>در صورت مطابقت true باز می‌گرداند</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);
}