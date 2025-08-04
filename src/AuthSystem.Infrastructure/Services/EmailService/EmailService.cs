using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Services.EmailService;

/// <summary>
/// پیاده‌سازی IEmailService با استفاده از MailKit
/// این کلاس برای ارسال ایمیل استفاده می‌شود
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="options">تنظیمات ایمیل</param>
    /// <param name="logger">ILogger</param>
    public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // اعتبارسنجی تنظیمات
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(_options);

        if (!Validator.TryValidateObject(_options, validationContext, validationResults, true))
        {
            var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidConfigurationException("Email", errors);
        }
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
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
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

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("ایمیل با موفقیت به {To} ارسال شد. موضوع: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ارسال ایمیل به '{To}' با موضوع '{Subject}' با خطا مواجه شد.", to, subject);
            throw new EmailSendingException(to, subject, ex);
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
        var subject = "خوش آمدید!";
        var body = $"""
            <h1>سلام {username}!</h1>
            <p>از ثبت نام شما در سیستم احراز هویت ما خوشحالیم.</p>
            <p>شما اکنون می‌توانید از خدمات ما استفاده کنید.</p>
            <p>با تشکر</p>
            """;

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
        var body = $"""
            <h1>درخواست بازیابی رمز عبور</h1>
            <p>شما درخواست بازیابی رمز عبور داده‌اید.</p>
            <p>برای بازیابی رمز عبور خود، روی لینک زیر کلیک کنید:</p>
            <p><a href="{resetLink}">بازیابی رمز عبور</a></p>
            <p>اگر این درخواست را نداده‌اید، می‌توانید این ایمیل را نادیده بگیرید.</p>
            """;

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
        var body = $"""
            <h1>تأیید آدرس ایمیل</h1>
            <p>برای تأیید آدرس ایمیل خود، روی لینک زیر کلیک کنید:</p>
            <p><a href="https://yoursite.com/confirm-email?token={confirmationToken}">تأیید ایمیل</a></p>
            <p>اگر این عمل را انجام نداده‌اید، می‌توانید این ایمیل را نادیده بگیرید.</p>
            """;

        await SendEmailAsync(to, subject, body, true);
    }
}