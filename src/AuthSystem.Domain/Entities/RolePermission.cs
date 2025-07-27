using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Entities
{
    /// <summary>
    /// Entity ارتباط Many-to-Many بین Role و Permission
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// شناسه نقش
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// شناسه مجوز
        /// </summary>
        public Guid PermissionId { get; set; }

        // Navigation Properties

        /// <summary>
        /// نقش مربوطه
        /// </summary>
        public Role Role { get; set; } = default!;

        /// <summary>
        /// مجوز مربوطه
        /// </summary>
        public Permission Permission { get; set; } = default!;
    }
}