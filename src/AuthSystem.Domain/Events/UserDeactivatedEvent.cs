using AuthSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Events
{
    public class UserDeactivatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public UserDeactivatedEvent(Guid userId) => UserId = userId;
    }
}
