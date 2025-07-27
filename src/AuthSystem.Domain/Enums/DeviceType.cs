using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Enums
{
    /// <summary>
    /// انواع دستگاه‌ها
    /// </summary>

    public enum DeviceType
    {
        /// <summary>
        /// نامشخص
        /// </summary>
        Unknown,

        /// <summary>
        /// رایانه شخصی یا لپ‌تاپ
        /// </summary>
        Desktop,

        /// <summary>
        /// گوشی همراه
        /// </summary>
        Mobile,

        /// <summary>
        /// تبلت
        /// </summary>
        Tablet
    }
}