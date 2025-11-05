
using System;

namespace AuthSystem.Domain.Common.Clock;

public static class DomainClock
{
    private static IDomainClock _current = new SystemDomainClock();

    public static IDomainClock Instance => _current;

    public static void Set(IDomainClock clock)
    {
        _current = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public static void Reset()
    {
        _current = new SystemDomainClock();
    }
}