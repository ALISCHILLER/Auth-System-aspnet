using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Events;

/// <summary>
/// اینترفیس برای رویدادهای دامنه ناهمزمان
/// </summary>
public interface IAsyncDomainEvent : IDomainEvent
{
    /// <summary>
    /// پردازش ناهمزمان رویداد
    /// </summary>
    Task HandleAsync();
}