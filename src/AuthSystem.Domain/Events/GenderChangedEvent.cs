using AuthSystem.Domain.Common;
using AuthSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Events
{
    public class GenderChangedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Gender NewGender { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public GenderChangedEvent(Guid userId, Gender newGender)
        {
            UserId = userId;
            NewGender = newGender;
        }
    }
}
