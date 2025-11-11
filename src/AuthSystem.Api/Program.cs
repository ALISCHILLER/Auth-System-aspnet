using System;
using AuthSystem.Api.Extensions;
using AuthSystem.Api.Filters;
using AuthSystem.Api.GraphQL;
using AuthSystem.Api.Hubs;
using AuthSystem.Api.Grpc;
using AuthSystem.Api.Middleware;
using AuthSystem.Application;
using AuthSystem.Application.Common.Options;
using AuthSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.Configure<PipelineLoggingOptions>(builder.Configuration.GetSection("Observability:PipelineLogging"));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

builder.Services.AddApiVersioningAndExplorer();
builder.Services.AddSwaggerDocs(builder.Configuration);
builder.Services.AddApiAuth(builder.Configuration);
builder.Services.AddApiProblemDetails();
builder.Services.AddApiRateLimiting(builder.Configuration);
builder.Services.AddApiTelemetry(builder.Configuration);
builder.Services.AddRealTimeSecurityEvents();
builder.Services.AddGrpc();
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddTypeExtension<SecurityEventQuery>()
    .AddType<SecurityEventGraphType>()
    .AddType<SecurityEventResultType>()
    .AddType<SecurityEventTypeEnumType>();

const string DefaultCorsPolicy = "DefaultCors";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

builder.Services.AddInfrastructureHealthChecks();

var app = builder.Build();

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSerilogRequestLogging();
app.UseCors(DefaultCorsPolicy);

app.UseApiRateLimiting();

if (app.Environment.IsDevelopment())
{
    app.UseVersionedSwaggerUI(apiVersionProvider);
}


app.UseHttpsRedirection();
app.UseApiProblemDetails();
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers().RequireCors(DefaultCorsPolicy);
app.MapGrpcService<AuthSystem.Api.Grpc.SecurityEventsGrpcService>().RequireAuthorization().RequireCors(DefaultCorsPolicy);
app.MapGraphQL().RequireAuthorization().RequireCors(DefaultCorsPolicy);
app.MapHub<SecurityEventsHub>("/hubs/security-events").RequireAuthorization().RequireCors(DefaultCorsPolicy);
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();