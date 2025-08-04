using AuthSystem.Domain.Exceptions.Infrastructure;
using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Services.SmsService;

/// <summary>
/// پیاده‌سازی ISmsService با استفاده از یک سرویس SMS خارجی
/// این کلاس برای ارسال پیامک استفاده می‌شود
/// </summary>
public class SmsService : ISmsService
{
    private readonly SmsOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SmsService> _logger;

    /// <summary>
    /// سازنده
    /// </summary>
    /// <param name="options">تنظیمات پیامک</param>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="logger">ILogger</param>
    public SmsService(IOptions<SmsOptions> options, HttpClient httpClient, ILogger<SmsService> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // اعتبارسنجی تنظیمات
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(_options);

        if (!Validator.TryValidateObject(_options, validationContext, validationResults, true))
        {
            var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidConfigurationException("Sms", errors);
        }
    }

    /// <summary>
    /// ارسال یک پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="message">متن پیام</param>
    /// <returns>تسک</returns>
    public async Task SendSmsAsync(string to, string message)
    {
        try
        {
            var smsRequest = new
            {
                apiKey = _options.ApiKey,
                sender = _options.SenderNumber,
                message = message,
                receptor = to
            };

            var json = JsonSerializer.Serialize(smsRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_options.ApiUrl, content);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("پیامک با موفقیت به {To} ارسال شد.", to);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "خطا در ارتباط با سرویس پیامک برای ارسال به '{To}'.", to);
            throw new SmsSendingException(to, message, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطای ناشناخته در ارسال پیامک به '{To}'.", to);
            throw new SmsSendingException(to, message, ex);
        }
    }

    /// <summary>
    /// ارسال کد تأیید پیامک
    /// </summary>
    /// <param name="to">شماره گیرنده</param>
    /// <param name="code">کد تأیید</param>
    /// <returns>تسک</returns>
    public async Task SendVerificationCodeAsync(string to, string code)
    {
        var message = $"کد تأیید شما: {code}";
        await SendSmsAsync(to, message);
    }
}