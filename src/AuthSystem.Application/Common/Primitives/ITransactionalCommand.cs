using MediatR;

namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// اینترفیس مارکر برای Commandهایی که باید در یک تراکنش دیتابیس اجرا شوند.
/// TransactionBehavior در پایپ‌لاین MediatR به دنبال این اینترفیس می‌گردد.
/// </summary>
/// <typeparam name="TResponse">نوع پاسخی که Command برمی‌گرداند.</typeparam>
public interface ITransactionalCommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// نسخه غیر-جنریک برای Commandهایی که Result برمی‌گردانند.
/// </summary>
public interface ITransactionalCommand : IRequest<Result>
{
}