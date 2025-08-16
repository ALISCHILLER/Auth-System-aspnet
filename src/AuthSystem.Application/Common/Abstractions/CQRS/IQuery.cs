using MediatR;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Abstractions.CQRS;

/// <summary>
/// نشانگر یک Query که نتیجه‌ای از نوع Result<T> را برمی‌گرداند
/// </summary>
/// <typeparam name="TResponse">نوع داده‌ای که در Result بازگردانده می‌شود</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
