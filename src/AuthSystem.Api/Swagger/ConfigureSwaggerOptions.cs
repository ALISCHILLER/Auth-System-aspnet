using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthSystem.Api.Swagger;

public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.CustomOperationIds(apiDescription =>
        {
            var controller = apiDescription.ActionDescriptor.RouteValues.TryGetValue("controller", out var value)
                ? value
                : "Endpoint";
            var method = apiDescription.HttpMethod ?? "GET";
            var path = (apiDescription.RelativePath ?? string.Empty)
                .Replace("/", "_")
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace(":", string.Empty);

            return $"{controller}_{method}_{path}";
        });

        options.DocumentFilter<SwaggerTagDocumentFilter>();
        options.OperationFilter<SwaggerDefaultResponseOperationFilter>();
        options.SchemaFilter<SwaggerEnumSchemaFilter>();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }

        var securityScheme = new OpenApiSecurityScheme
        {
            Scheme = "bearer",
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Input a valid JWT access token to access secured endpoints.",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() }
        });

        options.AddServer(new OpenApiServer
        {
            Url = "https://localhost:5001",
            Description = "Local development (HTTPS)"
        });

        options.AddServer(new OpenApiServer
        {
            Url = "https://auth.example.com",
            Description = "Production"
        });

        options.EnableAnnotations();
        options.SupportNonNullableReferenceTypes();
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "AuthSystem API",
            Version = description.ApiVersion.ToString(),
            Description =
                """
                <p>The AuthSystem API powers identity, access management, and auditing for multi-tenant SaaS products.</p>
                <p><strong>Highlights</strong></p>
                <ul>
                  <li>Standards compliant OAuth2 &amp; OpenID Connect token workflows with refresh rotation.</li>
                  <li>Fine-grained authorization via roles, permissions, and tenant-level policies.</li>
                  <li>Operational telemetry hooks including webhooks, SignalR streaming, gRPC, and GraphQL.</li>
                </ul>
                <p>Each operation returns structured envelopes (<code>ApiResponse&lt;T&gt;</code>) or RFC7807 problem details for errors.</p>
                """,
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "AuthSystem Platform",
                Email = "support@example.com",
                Url = new Uri("https://auth.example.com/support")
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        info.Extensions["x-api-lifecycle"] = new Microsoft.OpenApi.Any.OpenApiString(description.IsDeprecated ? "deprecated" : "active");

        return info;
    }
}