using AuthSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common;

/// <summary>
/// افزونه‌هایی برای تبدیل Result به IActionResult در ASP.NET Core
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// تبدیل Result به IActionResult
    /// </summary>
    public static IActionResult ToActionResult(this Result result, IStringLocalizer? localizer = null)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new
            {
                Status = result.Status,
                Message = localizer != null ? localizer[result.Message ?? "Success"] : result.Message
            });

        return result.WithProblemDetails(localizer);
    }

    /// <summary>
    /// تبدیل Result<T> به IActionResult
    /// </summary>
    public static IActionResult ToActionResult<T>(this Result<T> result, IStringLocalizer? localizer = null)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new
            {
                Status = result.Status,
                Message = localizer != null ? localizer[result.Message ?? "Success"] : result.Message,
                Data = result.Value
            });

        return result.WithProblemDetails(localizer);
    }

    /// <summary>
    /// تبدیل Result به ObjectResult با CustomProblemDetails
    /// </summary>
    private static ObjectResult WithProblemDetails(this Result result, IStringLocalizer? localizer)
    {
        var problemDetails = new CustomProblemDetails
        {
            Type = $"https://example.com/problems/{result.Status.ToString().ToLower()}",
            Title = localizer != null
                ? localizer[result.Status.ToString()]
                : result.Status switch
                {
                    AuthStatus.Success => "موفقیت",
                    AuthStatus.InvalidCredentials => "اعتبارسنجی ناموفق",
                    AuthStatus.AccountLocked => "حساب قفل شده",
                    AuthStatus.EmailNotConfirmed => "ایمیل تأیید نشده",
                    AuthStatus.PhoneNotConfirmed => "شماره تلفن تأیید نشده",
                    AuthStatus.TokenExpired => "توکن منقضی شده",
                    AuthStatus.InvalidToken => "توکن نامعتبر",
                    _ => "خطای داخلی سرور"
                },
            Status = result.Status switch
            {
                AuthStatus.Success => (int)HttpStatusCode.OK,
                AuthStatus.InvalidCredentials => (int)HttpStatusCode.BadRequest,
                AuthStatus.AccountLocked => (int)HttpStatusCode.Locked,
                AuthStatus.EmailNotConfirmed => (int)HttpStatusCode.BadRequest,
                AuthStatus.PhoneNotConfirmed => (int)HttpStatusCode.BadRequest,
                AuthStatus.TokenExpired => (int)HttpStatusCode.Unauthorized,
                AuthStatus.InvalidToken => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            },
            Detail = localizer != null ? localizer[result.Message ?? "An error occurred"] : result.Message,
            Errors = result.Errors,
            Instance = $"/api/errors/{result.Status}"
        };

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }
}
