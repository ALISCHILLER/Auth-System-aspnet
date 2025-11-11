using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthSystem.Api.Swagger;

/// <summary>
/// Ensures every operation surfaces the platform-wide error contracts and security requirements.
/// </summary>
public sealed class SwaggerDefaultResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                           context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true;

        if (hasAuthorize)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            var requiresBearer = operation.Security.All(requirement =>
                requirement.Keys.All(scheme => !string.Equals(scheme.Reference?.Id, "Bearer", StringComparison.OrdinalIgnoreCase)));

            if (requiresBearer)
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }] = Array.Empty<string>()
                });
            }
        }

        EnsureResponse(operation, "400", "Bad Request – validation or business rule failed.", nameof(ValidationProblemDetails));
        EnsureResponse(operation, "401", "Unauthorized – missing or invalid access token.");
        EnsureResponse(operation, "403", "Forbidden – caller lacks the required permissions.");
        EnsureResponse(operation, "429", "Too Many Requests – rate limit exceeded.");
        EnsureResponse(operation, "500", "Internal Server Error – unexpected failure.");
    }

    private static void EnsureResponse(OpenApiOperation operation, string statusCode, string description, string? schemaId = null)
    {
        if (operation.Responses.ContainsKey(statusCode))
        {
            return;
        }

        var response = new OpenApiResponse
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>()
        };

        schemaId ??= nameof(ProblemDetails);

        response.Content["application/json"] = new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.Schema,
                    Id = schemaId
                }
            }
        };

        operation.Responses[statusCode] = response;
    }
}