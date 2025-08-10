using System;
using System.Collections.Generic;

namespace AuthSystem.Application.DTOs.Responses;

/// <summary>
/// مدل پاسخ شامل اطلاعات نقش
/// </summary>
public class RoleResponse
{
    /// <summary>
    /// شناسه نقش
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// نام نقش
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// توضیحات نقش
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// نوع نقش (سیستمی یا سفارشی)
    /// </summary>
    public string RoleType { get; set; } = null!;

    /// <summary>
    /// تعداد کاربران دارای این نقش
    /// </summary>
    public int UserCount { get; set; }

    /// <summary>
    /// لیست مجوزهای مرتبط با نقش
    /// </summary>
    public IReadOnlyCollection<string> Permissions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// تاریخ آخرین به‌روزرسانی
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}