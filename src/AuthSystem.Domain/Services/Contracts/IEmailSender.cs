using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.Domain.Services.Contracts;

/// <summary>
/// اینترفیس برای ارسال ایمیل
/// این اینترفیس قراردادهای لازم برای ارسال ایمیل‌های سیستم را تعریف می‌کند
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// ارسال ایمیل ساده
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="subject">موضوع ایمیل</param>
    /// <param name="body">متن ایمیل</param>
    /// <param name="isHtml">آیا متن ایمیل HTML است</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendEmailAsync(
        Email to,
        string subject,
        string body,
        bool isHtml = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال ایمیل به چندین گیرنده
    /// </summary>
    /// <param name="tos">لیست آدرس‌های گیرنده</param>
    /// <param name="subject">موضوع ایمیل</param>
    /// <param name="body">متن ایمیل</param>
    /// <param name="isHtml">آیا متن ایمیل HTML است</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendEmailsAsync(
        IEnumerable<Email> tos,
        string subject,
        string body,
        bool isHtml = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال ایمیل تأیید ایمیل
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="verificationCode">کد تأیید</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendEmailVerificationAsync(
        Email to,
        string verificationCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال ایمیل بازیابی رمز عبور
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="resetToken">توکن بازیابی رمز عبور</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendPasswordResetAsync(
        Email to,
        string resetToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال ایمیل فعال‌سازی حساب
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="activationCode">کد فعال‌سازی</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendAccountActivationAsync(
        Email to,
        string activationCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ارسال ایمیل هشدار امنیتی
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="subject">موضوع ایمیل</param>
    /// <param name="details">جزئیات هشدار</param>
    /// <param name="cancellationToken">نشانگر لغو عملیات</param>
    /// <returns>تسک ناهمزمان</returns>
    Task SendSecurityAlertAsync(
        Email to,
        string subject,
        string details,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// آیا سرویس ایمیل فعال است
    /// </summary>
    /// <returns>وضعیت فعال بودن سرویس</returns>
    bool IsEnabled();
}