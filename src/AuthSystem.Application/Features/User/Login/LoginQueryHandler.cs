using AuthSystem.Application.Common;
using AuthSystem.Application.DTOs.Responses;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Events;
using AutoMapper;
using MediatR;
using System.Security.Claims;

namespace AuthSystem.Application.Features.User.Login;

/// <summary>
/// پردازش‌گر پرس‌وجو ورود به سیستم
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<Result<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // دریافت کاربر بر اساس ایمیل یا نام کاربری
        var user = await GetUserAsync(request.UsernameOrEmail, cancellationToken);
        if (user == null)
            return Result<LoginResponse>.Failed(AuthStatus.InvalidCredentials, "ایمیل یا نام کاربری نامعتبر است");

        // بررسی فعال بودن حساب
        if (!user.IsActive)
            return Result<LoginResponse>.Failed(AuthStatus.AccountLocked, "حساب کاربر قفل شده است");

        // بررسی قفل خودکار
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Result<LoginResponse>.Failed(AuthStatus.AccountLocked, "حساب به دلیل تلاش‌های ناموفق قفل شده است");

        // بررسی رمز عبور
        if (!_passwordService.Verify(request.Password, user.PasswordHash))
        {
            user.IncrementFailedLoginAttempts();
            await _userRepository.UpdateAsync(user, cancellationToken);
            return Result<LoginResponse>.Failed(AuthStatus.InvalidCredentials, "رمز عبور نامعتبر است");
        }

        // ریست تلاش‌های ناموفق
        if (user.FailedLoginAttempts > 0)
        {
            user.ResetFailedLoginAttempts();
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // ساخت claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim("Gender", user.Gender.ToString())
        };

        // تولید توکن
        var accessToken = _jwtService.GenerateToken(claims, request.ClientInfo);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id, request.ClientInfo);

        // به‌روزرسانی زمان آخرین ورود
        user.SetLastLoginAt(DateTime.UtcNow);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // ارسال رویداد ورود
        user.AddDomainEvent(new UserLoggedInEvent(user.Id, user.EmailAddress, request.ClientInfo));

        // مپ کردن به LoginResponse
        var response = _mapper.Map<LoginResponse>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = refreshToken;
        response.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

        return Result<LoginResponse>.Succeeded(response, AuthStatus.Success, "ورود با موفقیت انجام شد");
    }

    private async Task<User?> GetUserAsync(string usernameOrEmail, CancellationToken cancellationToken)
    {
        if (usernameOrEmail.Contains("@"))
            return await _userRepository.GetByEmailAsync(usernameOrEmail, cancellationToken);

        return await _userRepository.GetByUsernameAsync(usernameOrEmail, cancellationToken);
    }
}