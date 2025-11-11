using AuthSystem.Api.Middleware;

namespace AuthSystem.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<ExceptionHandlingMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseApiProblemDetails(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}