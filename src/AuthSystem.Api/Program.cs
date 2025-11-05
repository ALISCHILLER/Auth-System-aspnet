using AuthSystem.Api.Extensions;
using AuthSystem.Api.Filters;
using AuthSystem.Application;
using AuthSystem.Infrastructure;
using Serilog;
using Serilog.Events;
using AuthSystem.Infrastructure.Extensions;

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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddInfrastructureHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseApiProblemDetails();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();