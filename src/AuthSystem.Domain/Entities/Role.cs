using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity نقش
    /// </summary>
    public class Role : BaseEntity
    {

        /// <summary>
        /// نام نقش (unik)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// توضیحات نقش
        /// </summary>
        [MaxLength(255)]
        public string? Description { get; set; }

        // Navigation Properties
        /// <summary>
        /// لیست کاربران دارای این نقش
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        /// <summary>
        /// لیست مجوزهای این نقش
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        
    }
}