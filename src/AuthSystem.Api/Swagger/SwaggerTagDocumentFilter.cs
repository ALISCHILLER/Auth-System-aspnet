using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthSystem.Api.Swagger;

/// <summary>
/// Provides curated tag metadata so the rendered Swagger UI offers contextual grouping.
/// </summary>
public sealed class SwaggerTagDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = new List<OpenApiTag>
        {
            new()
            {
                Name = "Authentication",
                Description = "Access tokens, refresh rotation, multi-factor verification, and logout flows.",
                ExternalDocs = new OpenApiExternalDocs
                {
                    Description = "Authentication quickstart",
                    Url = new Uri("https://auth.example.com/docs/authentication")
                }
            },
            new()
            {
                Name = "Users",
                Description = "Registration, profile retrieval, SCIM provisioning, and tenant enrolment endpoints.",
                ExternalDocs = new OpenApiExternalDocs
                {
                    Description = "Managing users",
                    Url = new Uri("https://auth.example.com/docs/users")
                }
            },
            new()
            {
                Name = "Roles",
                Description = "Role lifecycle management and assignment APIs for coarse-grained authorization.",
                ExternalDocs = new OpenApiExternalDocs
                {
                    Description = "Authorization guide",
                    Url = new Uri("https://auth.example.com/docs/authorization")
                }
            },
            new()
            {
                Name = "Audit",
                Description = "Security event history, webhook subscriptions, and real-time notifications.",
                ExternalDocs = new OpenApiExternalDocs
                {
                    Description = "Observability patterns",
                    Url = new Uri("https://auth.example.com/docs/audit")
                }
            }
        };
    }
}