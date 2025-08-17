using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Enums;

/// <summary>
/// سطح امنیتی
/// </summary>
public enum SecurityLevel
{
    /// <summary>
    /// سطح پایین
    /// </summary>
    Low,

    /// <summary>
    /// سطح متوسط
    /// </summary>
    Medium,

    /// <summary>
    /// سطح بالا
    /// </summary>
    High,

    /// <summary>
    /// سطح بحرانی
    /// </summary>
    Critical
}
