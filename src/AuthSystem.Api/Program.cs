using Serilog;
using AuthSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration
// پیکربندی Serilog برای لاگ‌نویسی
builder.Host.UseSerilog((context, services, configuration) => // نام پارامتر دوم باید 'services' باشد نه 'service'
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services) // اینجا هم باید 'services' باشد
);
#endregion

#region EF Core Configuration
// پیکربندی Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // در اینجا می‌توانید بین PostgreSQL و SQL Server انتخاب کنید
    // برای PostgreSQL:
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    // برای SQL Server:
    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
#endregion

#region Repository Registration
// ثبت Repositoryها
builder.Services.AddScoped<AuthSystem.Domain.Repositories.IUserRepository, AuthSystem.Infrastructure.Repositories.UserRepository>();
// می‌تونید سایر Repositoryها رو هم به همین ترتیب اضافه کنید
// builder.Services.AddScoped<AuthSystem.Domain.Repositories.IRoleRepository, AuthSystem.Infrastructure.Repositories.RoleRepository>();
builder.Services.AddScoped(typeof(AuthSystem.Domain.Repositories.IGenericRepository<>), typeof(AuthSystem.Infrastructure.Repositories.GenericRepository<>));
#endregion


// اضافه کردن کنترلرها
builder.Services.AddControllers();

// اضافه کردن قابلیت‌های مورد نیاز برای Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // تصحیح: 'Enviroment' به 'Environment'
{
    // فعال کردن Swagger در محیط توسعه
    app.UseSwagger();
    app.UseSwaggerUI();
}

// انتقال به HTTPS
app.UseHttpsRedirection();

// توجه: میدلویرهای احراز هویت و اجازه‌دهی بعداً اضافه خواهند شد
// app.UseAuthentication();
// app.UseAuthorization();

// نگاشت کنترلرها به مسیرها
app.MapControllers();

// اجرای اپلیکیشن
app.Run();