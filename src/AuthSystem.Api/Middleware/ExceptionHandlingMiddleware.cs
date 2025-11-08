using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Shared.Contracts;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            await next(context).ConfigureAwait(false);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, "Unauthorized", "unauthorized").ConfigureAwait(false);
        }
        catch (ForbiddenException exception)
        {
            await WriteProblemAsync(context, HttpStatusCode.Forbidden, exception.Message, "forbidden").ConfigureAwait(false);
        }
        catch (NotFoundException exception)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, exception.Message, "not_found").ConfigureAwait(false);
        }
        catch (AppValidationException exception)
        {
            var errors = exception.Errors
                  .Select(pair => new ApiError(pair.Key, string.Join("; ", pair.Value)))
                  .ToArray();

            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Validation failed", "validation_failed", errors)
                 .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(exception, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", "internal_error")
                .ConfigureAwait(false);
        }
    }

    private static Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        string code,
        ApiError[]? errors = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var problem = new ProblemDetails
        {
            Title = message,
            Status = (int)statusCode,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5",
            Instance = context.Request.Path,
            Detail = errors is null ? message : "One or more errors occurred."
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        problem.Extensions["code"] = code;
        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        var payload = JsonSerializer.Serialize(problem, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        return context.Response.WriteAsync(payload);
    }
}