using AuthSystem.Application;
using AuthSystem.Infrastructure;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
   // Layer registrations
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// MVC + Swagger + HealthChecks
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Enable these when real authentication is wired up.
// app.UseAuthentication();
// app.UseAuthorization();

// نگاشت کنترلرها به مسیرها
app.MapControllers();
app.MapHealthChecks("/health");
// اجرای اپلیکیشن
app.Run();