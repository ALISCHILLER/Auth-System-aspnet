using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای ارسال کد تأیید احراز هویت دو عاملی
/// این اینترفیس قراردادهای لازم برای ارسال کدهای تأیید 2FA را تعریف می‌کند
/// </summary>
public interface ITwoFactorCodeSender
{
    /// <summary>
    /// ارسال کد تأیید از طریق ایمیل
    /// </summary>
    /// <param name="email">آدرس ایمیل گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendByEmailAsync(Email email, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال کد تأیید از طریق پیامک
    /// </summary>
    /// <param name="phoneNumber">شماره تلفن گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendBySmsAsync(PhoneNumber phoneNumber, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال کد تأیید از طریق اپلیکیشن‌های احراز هویت
    /// </summary>
    /// <param name="email">آدرس ایمیل یا شماره تلفن گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendByAuthenticatorAsync(Email email, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال کد تأیید از طریق تماس صوتی
    /// </summary>
    /// <param name="phoneNumber">شماره تلفن گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendByVoiceCallAsync(PhoneNumber phoneNumber, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال کد تأیید از طریق روش‌های متعدد
    /// </summary>
    /// <param name="verificationCodeType">نوع کد تأیید</param>
    /// <param name="identifier">شناسه گیرنده (ایمیل یا شماره تلفن)</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendAsync(VerificationCodeType verificationCodeType, string identifier, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال کد تأیید احراز هویت دو عاملی
    /// </summary>
    /// <param name="twoFactorMethod">روش احراز هویت دو عاملی</param>
    /// <param name="identifier">شناسه گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendTwoFactorCodeAsync(AuthenticationMethod twoFactorMethod, string identifier, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// آیا روش ارسال کد تأیید فعال است
    /// </summary>
    /// <param name="method">روش ارسال</param>
    /// <returns>وضعیت فعال بودن روش</returns>
    bool IsMethodEnabled(AuthenticationMethod method);

    /// <summary>
    /// دریافت لیست روش‌های ارسال کد تأیید فعال
    /// </summary>
    /// <returns>لیست روش‌های فعال</returns>
    AuthenticationMethod[] GetEnabledMethods();

    /// <summary>
    /// آیا سرویس ارسال کد تأیید فعال است
    /// </summary>
    /// <returns>وضعیت فعال بودن سرویس</returns>
    bool IsEnabled();
}