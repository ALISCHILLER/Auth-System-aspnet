namespace AuthSystem.Domain.Common.Clock;

public sealed class SystemDomainClock : IDomainClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}