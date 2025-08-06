using AuthSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Events
{
    public class ProfileImageChangedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string NewImageUrl { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public ProfileImageChangedEvent(Guid userId, string newImageUrl)
        {
            UserId = userId;
            NewImageUrl = newImageUrl;
        }
    }
}
