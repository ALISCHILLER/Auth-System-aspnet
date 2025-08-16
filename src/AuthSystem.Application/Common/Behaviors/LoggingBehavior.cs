using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Application.Common.Behaviors
{
    /// <summary>
    /// رفتار لاگ‌برداری درخواست‌ها و زمان اجرای آن‌ها.
    /// شامل سریالایز امن Payload و ثبت زمان اجرا.
    /// </summary>
    /// <typeparam name="TRequest">نوع درخواست</typeparam>
    /// <typeparam name="TResponse">نوع پاسخ</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            string payload;
            try
            {
                payload = JsonSerializer.Serialize(request, _jsonOptions);
            }
            catch (Exception ex)
            {
                payload = "<unserializable>";
                _logger.LogDebug(ex, "سریالایز کردن {RequestName} ناموفق.", requestName);
            }

            _logger.LogInformation("شروع {RequestName} | Payload: {Payload}", requestName, payload);

            var sw = Stopwatch.StartNew();
            try
            {
                var response = await next();
                sw.Stop();

                _logger.LogInformation("پایان {RequestName} | مدت: {Elapsed} ms", requestName, sw.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "خطا در {RequestName} | مدت: {Elapsed} ms", requestName, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
