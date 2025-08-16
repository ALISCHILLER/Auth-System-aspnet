using MediatR;
using AuthSystem.Application.Common.Primitives;

namespace AuthSystem.Application.Common.Abstractions.CQRS;

/// <summary>
/// نشانگر یک Command که نتیجه‌ای از نوع Result را برمی‌گرداند
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// نشانگر یک Command که نتیجه‌ای از نوع Result<T> را برمی‌گرداند
/// </summary>
/// <typeparam name="TResponse">نوع داده‌ای که در Result بازگردانده می‌شود</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
