using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity ارتباط Many-to-Many بین User و Role
    /// </summary>

    public class UserRole
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// شناسه نقش
        /// </summary>
        public Guid RoleId { get; set; }

        // Navigation Properties

        /// <summary>
        /// کاربر مربوطه
        /// </summary>
        public User User { get; set; } = default!;

        /// <summary>
        /// نقش مربوطه
        /// </summary>
        public Role Role { get; set; } = default!;
    }
}