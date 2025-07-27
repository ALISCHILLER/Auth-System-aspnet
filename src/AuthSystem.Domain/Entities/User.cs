using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity کاربر
    /// </summary>

    public class User : BaseEntity
    {
        /// <summary>
        /// نام کاربری (unik)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// ایمیل (unik)
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// آیا ایمیل تأیید شده است؟
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// شماره تلفن
        /// </summary>
        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// آیا شماره تلفن تأیید شده است؟
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// هش رمز عبور
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// URL تصویر پروفایل
        /// </summary>
        public string? ProfileImageUrl { get; set; }

        /// <summary>
        /// آیا کاربر فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// تاریخ آخرین ورود
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        // Navigation Properties

        /// <summary>
        /// لیست نقش‌های کاربر
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        /// <summary>
        /// لیست توکن‌های تازه‌سازی کاربر
        /// </summary>
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        /// <summary>
        /// تاریخچه ورودهای کاربر
        /// </summary>
        public ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

        /// <summary>
        /// لیست دستگاه‌های کاربر
        /// </summary>
        public ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
    }
}