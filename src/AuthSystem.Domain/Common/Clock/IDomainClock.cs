using System;
namespace AuthSystem.Domain.Common.Clock;

public interface IDomainClock
{
    DateTime UtcNow { get; }
}