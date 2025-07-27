using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity تاریخچه ورود کاربران
    /// </summary>
    public class LoginHistory : BaseEntity
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
        /// تاریخ و زمان ورود
        /// </summary>
        public DateTime LoginAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// آدرس IP که از آن وارد شده
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// اطلاعات User Agent (مرورگر/دستگاه)
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// آیا ورود موفق بوده؟
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// دلیل ناموفق بودن ورود (در صورت وجود)
        /// </summary>
        public string? FailureReason { get; set; }
    }
}