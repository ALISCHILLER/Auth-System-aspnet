// File: AuthSystem.Domain/Common/Events/DomainEventMetadata.cs
using System;

namespace AuthSystem.Domain.Common.Events
{
    /// <summary>
    /// متادیتای استاندارد رویداد دامنه جهت ردیابی و نسخه‌بندی.
    /// </summary>
    public sealed class DomainEventMetadata
    {
        public Guid EventId { get; init; }
        public string EventType { get; init; } = string.Empty;
        public int EventVersion { get; init; }
        public DateTime OccurredOn { get; init; }
        public string? TriggeredBy { get; init; }
        public Guid CorrelationId { get; init; }
    }
}