// File: AuthSystem.Domain/Enums/SecurityLevel.cs
namespace AuthSystem.Domain.Enums;

/// <summary>
/// سطوح امنیتی مختلف در سیستم
/// - این enum برای تعیین سطح امنیتی عملیات‌ها استفاده می‌شود
/// </summary>
public enum SecurityLevel
{
    /// <summary>
    /// سطح امنیتی پایین (برای عملیات‌های غیرحساس)
    /// </summary>
    Low = 1,

    /// <summary>
    /// سطح امنیتی متوسط (برای عملیات‌های معمولی)
    /// </summary>
    Medium = 2,

    /// <summary>
    /// سطح امنیتی بالا (برای عملیات‌های حساس)
    /// </summary>
    High = 3,

    /// <summary>
    /// سطح امنیتی بسیار بالا (برای عملیات‌های بسیار حساس)
    /// </summary>
    Critical = 4,

    /// <summary>
    /// سطح امنیتی موقت (برای زمان‌های خاص)
    /// </summary>
    Temporary = 5
}