using AuthSystem.Api.Extensions;
using AuthSystem.Api.Filters;
using AuthSystem.Api.GraphQL;
using AuthSystem.Api.Hubs;
using AuthSystem.Api.Grpc;
using AuthSystem.Api.Middleware;
using AuthSystem.Application;
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddInfrastructureHealthChecks();

var app = builder.Build();

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSerilogRequestLogging();
app.UseCors();

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


app.MapControllers();
app.MapGrpcService<AuthSystem.Api.Grpc.SecurityEventsGrpcService>().RequireAuthorization();
app.MapGraphQL().RequireAuthorization();
app.MapHub<SecurityEventsHub>("/hubs/security-events").RequireAuthorization();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();