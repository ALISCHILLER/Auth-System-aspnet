using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common;

/// <summary>
/// مدل سفارشی برای Problem Details (RFC 7807) با پشتیبانی از لیست خطاها
/// </summary>
public class CustomProblemDetails
{
    /// <summary>
    /// لینک نوع مشکل
    /// </summary>
    public string Type { get; set; } = "https://example.com/problems";

    /// <summary>
    /// عنوان خلاصه مشکل
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// کد وضعیت HTTP
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// توضیحات مشخص برای این رخداد
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// لینک خاص به رخداد (مثلاً /api/errors/InvalidCredentials)
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    /// لیست خطاهای اعتبارسنجی یا عملیاتی
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();
}