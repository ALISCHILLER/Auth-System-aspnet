using AuthSystem.Domain.Interfaces.Repositories;
using AuthSystem.Domain.Interfaces.Services;
using AuthSystem.Infrastructure.Persistence;
using AuthSystem.Infrastructure.Persistence.Contexts;
using AuthSystem.Infrastructure.Persistence.Repositories;
using AuthSystem.Infrastructure.Services;
using AuthSystem.Infrastructure.Services.EmailService;
using AuthSystem.Infrastructure.Services.JwtService;
using AuthSystem.Infrastructure.Services.PasswordHasher;
using AuthSystem.Infrastructure.Services.SmsService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem.Infrastructure.Common;

/// <summary>
/// کلاس کمکی برای ثبت وابستگی‌ها در سیستم DI
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// ثبت سرویس‌های مربوط به پایداری داده (Persistence)
    /// </summary>
    /// <param name="services">سرویس کالکشن</param>
    /// <param name="configuration">کانفیگوریشن</param>
    /// <returns>سرویس کالکشن</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // ثبت DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ثبت Repositoryها
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    /// <summary>
    /// ثبت سرویس‌های زیرساختی (Infrastructure)
    /// </summary>
    /// <param name="services">سرویس کالکشن</param>
    /// <param name="configuration">کانفیگوریشن</param>
    /// <returns>سرویس کالکشن</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ثبت سرویس‌های دامنه
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator>(provider =>
        {
            var jwtSettings = configuration.GetSection("Jwt");
            return new JwtService(
                secretKey: jwtSettings["SecretKey"],
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                accessTokenExpirationMinutes: int.Parse(jwtSettings["AccessTokenExpirationMinutes"]),
                refreshTokenExpirationDays: int.Parse(jwtSettings["RefreshTokenExpirationDays"])
            );
        });
        services.AddScoped<IEmailService>(provider =>
        {
            var emailSettings = configuration.GetSection("Email");
            return new EmailService(
                smtpServer: emailSettings["SmtpServer"],
                smtpPort: int.Parse(emailSettings["SmtpPort"]),
                smtpUsername: emailSettings["SmtpUsername"],
                smtpPassword: emailSettings["SmtpPassword"],
                fromEmail: emailSettings["FromEmail"],
                fromName: emailSettings["FromName"]
            );
        });
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}