using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity دستگاه‌های کاربر
    /// </summary>
    public class UserDevice : BaseEntity
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// کاربر مربوطه
        /// </summary>
        public User User { get; set; } = default!;

        /// <summary>
        /// شناسه یکتای دستگاه (Fingerprint یا Device Identifier)
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// نام دستگاه (اختیاری)
        /// </summary>
        [MaxLength(100)]
        public string? DeviceName { get; set; }

        /// <summary>
        /// نوع دستگاه (مثلاً "Mobile", "Desktop")
        /// </summary>
        [MaxLength(50)]
        public string? DeviceType { get; set; }

        /// <summary>
        /// نام سیستم عامل
        /// </summary>
        [MaxLength(50)]
        public string? OsName { get; set; }

        /// <summary>
        /// اطلاعات مرورگر
        /// </summary>
        [MaxLength(255)]
        public string? BrowserInfo { get; set; }

        /// <summary>
        /// آخرین آدرس IP مورد استفاده از این دستگاه
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// تاریخ آخرین ورود از این دستگاه
        /// </summary>
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// آیا این دستگاه مورد اعتماد است؟
        /// </summary>
        public bool IsTrusted { get; set; } = false;
    }
}