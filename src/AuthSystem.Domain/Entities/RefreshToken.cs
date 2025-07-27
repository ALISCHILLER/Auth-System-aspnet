using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity توکن تازه‌سازی برای مدیریت جلسات کاربری
    /// </summary>

    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// مقدار توکن (unik)
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// شناسه کاربر صاحب توکن
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// کاربر صاحب توکن
        /// </summary>
        public User User { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضای توکن
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// تاریخ ابطال توکن (null یعنی فعال است)
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// آیا توکن هنوز فعال است؟
        /// </summary>
        public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;

        /// <summary>
        /// آدرس IP که توکن از آن درخواست شده
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// اطلاعات User Agent (مرورگر/دستگاه)
        /// </summary>
        public string? UserAgent { get; set; }
    }
}