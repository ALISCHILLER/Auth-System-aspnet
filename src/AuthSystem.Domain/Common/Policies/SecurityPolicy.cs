using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.Services.Contracts;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// سیاست‌های امنیتی سیستم
/// </summary>
public static class SecurityPolicy
{
    /// <summary>
    /// سیاست بررسی دسترسی به منابع
    /// </summary>
    public static IPolicy<AuthorizationContext> ResourceAccessPolicy =>
        new ResourceAccessPolicy();

    /// <summary>
    /// سیاست بررسی تعداد تلاش‌های ورود
    /// </summary>
    public static IPolicy<LoginContext> LoginAttemptPolicy =>
        new LoginAttemptPolicy();

    /// <summary>
    /// سیاست بررسی انقضای رمز عبور
    /// </summary>
    public static IPolicy<User> PasswordExpirationPolicy =>
        new PasswordExpirationPolicy();

    /// <summary>
    /// سیاست بررسی محدودیت نرخ درخواست
    /// </summary>
    public static IAsyncPolicy<RequestContext> RateLimitPolicy =>
        new RateLimitPolicy();

    /// <summary>
    /// سیاست بررسی تغییرات حساس
    /// </summary>
    public static IPolicy<SecurityContext> SensitiveOperationPolicy =>
        new SensitiveOperationPolicy();

    private class ResourceAccessPolicy : BasePolicy<AuthorizationContext>
    {
        public override PolicyResult Evaluate(AuthorizationContext context)
        {
            if (context.User == null)
                return PolicyResult.Failure("کاربر نامعتبر است");

            if (string.IsNullOrEmpty(context.RequiredPermission))
                return PolicyResult.Success();

            var hasPermission = context.User.HasPermission(context.RequiredPermission);

            return hasPermission
                ? PolicyResult.Success()
                : PolicyResult.Failure($"دسترسی به {context.Resource} نیاز به مجوز {context.RequiredPermission} دارد");
        }
    }

    private class LoginAttemptPolicy : BasePolicy<LoginContext>
    {
        public override PolicyResult Evaluate(LoginContext context)
        {
            if (context.FailedAttempts >= 5)
            {
                var lockoutTime = DateTime.UtcNow.AddMinutes(15);
                return PolicyResult.Failure($"حساب به مدت 15 دقیقه قفل شده است. لطفاً بعد از {lockoutTime:HH:mm} مجدداً تلاش کنید");
            }

            return PolicyResult.Success();
        }
    }

    private class PasswordExpirationPolicy : BasePolicy<User>
    {
        public override PolicyResult Evaluate(User user)
        {
            if (user.IsPasswordExpired())
            {
                return PolicyResult.Failure("رمز عبور شما منقضی شده است. لطفاً رمز عبور خود را تغییر دهید");
            }

            return PolicyResult.Success();
        }
    }

    private class RateLimitPolicy : BasePolicy<RequestContext>, IAsyncPolicy<RequestContext>
    {
        private readonly IRateLimiter _rateLimiter;

        public RateLimitPolicy(IRateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        public override PolicyResult Evaluate(RequestContext context)
        {
            var result = _rateLimiter.CheckLimit(context.IpAddress);
            return result.IsAllowed
                ? PolicyResult.Success()
                : PolicyResult.Failure($"تعداد درخواست‌ها بیش از حد مجاز است. لطفاً {result.Reset - DateTime.UtcNow} ثانیه صبر کنید");
        }

        public Task<PolicyResult> EvaluateAsync(RequestContext context)
        {
            return Task.FromResult(Evaluate(context));
        }
    }

    private class SensitiveOperationPolicy : BasePolicy<SecurityContext>
    {
        public override PolicyResult Evaluate(SecurityContext context)
        {
            if (context.OperationType == SecurityOperationType.DeleteAccount &&
                !context.User.IsAdmin)
            {
                return PolicyResult.Failure("حذف حساب نیاز به دسترسی ادمین دارد");
            }

            return PolicyResult.Success();
        }
    }

    /// <summary>
    /// کلاس‌های context برای سیاست‌ها
    /// </summary>
    public class AuthorizationContext
    {
        public ClaimsPrincipal User { get; set; }
        public string Resource { get; set; }
        public string RequiredPermission { get; set; }
    }

    public class LoginContext
    {
        public string Username { get; set; }
        public int FailedAttempts { get; set; }
    }

    public class RequestContext
    {
        public string IpAddress { get; set; }
        public string Endpoint { get; set; }
    }

    public class SecurityContext
    {
        public User User { get; set; }
        public SecurityOperationType OperationType { get; set; }
    }

    public enum SecurityOperationType
    {
        DeleteAccount,
        ChangeRole,
        ModifyPermissions,
        ViewSensitiveData
    }
}