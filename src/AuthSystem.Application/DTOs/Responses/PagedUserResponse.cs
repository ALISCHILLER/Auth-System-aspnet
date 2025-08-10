using System.Collections.Generic;

namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ برای لیست کاربران با صفحه‌بندی
/// </summary>
public class PagedUserResponse
{
    /// <summary>
    /// لیست کاربران
    /// </summary>
    public IReadOnlyCollection<UserResponse> Users { get; set; } = Array.Empty<UserResponse>();

    /// <summary>
    /// شماره صفحه فعلی
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// تعداد آیتم‌ها در هر صفحه
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// تعداد کل کاربران
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// تعداد کل صفحات
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}