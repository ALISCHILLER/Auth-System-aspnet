using AuthSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Events
{
    public class UserActivatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public UserActivatedEvent(Guid userId) => UserId = userId;
    }
}
