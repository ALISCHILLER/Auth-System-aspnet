using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace AuthSystem.Domain.Common.Behaviors;

/// <summary>
/// رفتار مدیریت استثناهای ناگهانی برای پیپ‌لاین MediatR
/// این رفتار هرگونه استثنای ناگهانی را ثبت کرده و به صورت مناسب پردازش می‌کند
/// </summary>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// اجرای رفتار مدیریت استثنا
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            // ثبت خطا در سیستم لاگ
            Log.Error(ex, "Unhandled exception for request {Request}", typeof(TRequest).Name);

            // پردازش استثنا بر اساس نوع آن
            var message = $"An unhandled exception has occurred while processing the request: {ex.Message}";

            // در محیط توسعه می‌توان جزئیات بیشتری نمایش داد
#if DEBUG
            message += $"\nStack Trace: {ex.StackTrace}";
#endif

            // ایجاد استثنا مناسب
            throw new ApplicationException(message, ex);
        }
    }
}