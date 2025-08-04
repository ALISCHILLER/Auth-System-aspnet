using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Options;
using AuthSystem.Infrastructure.Services;
using AuthSystem.Infrastructure.Services.EmailService;
using AuthSystem.Infrastructure.Services.JwtService;
using AuthSystem.Infrastructure.Services.PasswordHasher;
using AuthSystem.Infrastructure.Services.SmsService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem.Infrastructure.Extensions;

/// <summary>
/// اکستنشن برای ثبت Services در DI
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// ثبت Services در DI
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ثبت تنظیمات
        services.Configure<CryptoOptions>(configuration.GetSection(CryptoOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.Configure<SmsOptions>(configuration.GetSection(SmsOptions.SectionName));

        // ثبت سرویس‌ها
        services.AddScoped<ICryptoService, CryptoService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}