using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using AuthSystem.Application.Common.Exceptions;

namespace AuthSystem.Api.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (AppValidationException ex)
        {
            var payload = new
            {
                title = "Validation failed",
                status = (int)HttpStatusCode.BadRequest,
                errors = ex.Errors
            };

            await WriteProblemAsync(context, HttpStatusCode.BadRequest, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, object payload)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var json = payload is string message
            ? JsonSerializer.Serialize(new { title = message, status = (int)statusCode })
            : JsonSerializer.Serialize(payload);

        return context.Response.WriteAsync(json);
    }
}