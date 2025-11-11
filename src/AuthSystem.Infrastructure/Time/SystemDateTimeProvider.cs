using AuthSystem.Application.Common.Abstractions.Time;

namespace AuthSystem.Infrastructure.Time;

internal sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}