using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity مجوز
    /// </summary>
    public class Permission : BaseEntity
    {
        /// <summary>
        /// نام مجوز (unik)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// توضیحات مجوز
        /// </summary>
        [MaxLength(255)]
        public string? Description { get; set; }

        // Navigation Properties

        /// <summary>
        /// لیست نقش‌هایی که این مجوز را دارند
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}