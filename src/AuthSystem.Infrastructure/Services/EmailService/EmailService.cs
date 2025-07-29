using AuthSystem.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Services.EmailService;

/// <summary>
/// پیاده‌سازی IEmailService با استفاده از MailKit
/// </summary>
public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="smtpServer">آدرس سرور SMTP</param>
    /// <param name="smtpPort">پورت SMTP</param>
    /// <param name="smtpUsername">نام کاربری SMTP</param>
    /// <param name="smtpPassword">رمز عبور SMTP</param>
    /// <param name="fromEmail">آدرس ایمیل فرستنده</param>
    /// <param name="fromName">نام فرستنده</param>
    public EmailService(
        string smtpServer,
        int smtpPort,
        string smtpUsername,
        string smtpPassword,
        string fromEmail,
        string fromName)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
        _fromEmail = fromEmail;
        _fromName = fromName;
    }

    /// <summary>
    /// ارسال یک ایمیل
    /// </summary>
    /// <param name="to">گیرنده</param>
    /// <param name="subject">موضوع</param>
    /// <param name="body">متن</param>
    /// <param name="isHtml">آیا متن HTML است؟</param>
    /// <returns>تسک</returns>
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_fromName, _fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        if (isHtml)
        {
            bodyBuilder.HtmlBody = body;
        }
        else
        {
            bodyBuilder.TextBody = body;
        }
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, true);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    /// <summary>
    /// ارسال ایمیل خوش‌آمدگویی
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="username">نام کاربری</param>
    /// <returns>تسک</returns>
    public async Task SendWelcomeEmailAsync(string to, string username)
    {
        var subject = "خوش آمدید به سیستم احراز هویت";
        var body = $"<h1>سلام {username}!</h1><p>از ثبت نام شما در سیستم احراز هویت متشکریم.</p>";
        await SendEmailAsync(to, subject, body, true);
    }

    /// <summary>
    /// ارسال ایمیل بازیابی رمز عبور
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="resetLink">لینک بازیابی</param>
    /// <returns>تسک</returns>
    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        var subject = "بازیابی رمز عبور";
        var body = $"<h1>بازیابی رمز عبور</h1><p>برای بازیابی رمز عبور خود، روی لینک زیر کلیک کنید:</p><p><a href='{resetLink}'>بازیابی رمز عبور</a></p>";
        await SendEmailAsync(to, subject, body, true);
    }

    /// <summary>
    /// ارسال ایمیل تأیید ایمیل
    /// </summary>
    /// <param name="to">آدرس گیرنده</param>
    /// <param name="confirmationToken">توکن تأیید</param>
    /// <returns>تسک</returns>
    public async Task SendEmailConfirmationEmailAsync(string to, string confirmationToken)
    {
        var subject = "تأیید آدرس ایمیل";
        var body = $"<h1>تأیید آدرس ایمیل</h1><p>برای تأیید آدرس ایمیل خود، روی لینک زیر کلیک کنید:</p><p><a href='{confirmationToken}'>تأیید ایمیل</a></p>";
        await SendEmailAsync(to, subject, body, true);
    }
}