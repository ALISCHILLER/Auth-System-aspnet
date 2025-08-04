using AuthSystem.Domain.Entities;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AuthSystem.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync(); // اجرای خودکار migrationها

        // افزودن نقش‌های پیش‌فرض
        if (!context.Roles.Any())
        {
            var adminRole = Role.Create("Admin", "مدیر سیستم");
            var userRole = Role.Create("User", "کاربر عادی");
            var supportRole = Role.Create("Support", "پشتیبانی");

            context.Roles.AddRange(adminRole, userRole, supportRole);
            await context.SaveChangesAsync();
        }

        // افزودن کاربر ادمین پیش‌فرض
        if (!context.Users.Any())
        {
            var adminRole = context.Roles.First(r => r.Name == "Admin");

            var email = Email.Create("admin@example.com").Value;
            var password = Password.Create("SecurePa$$w0rd").Value;
            var phone = PhoneNumber.Create("09121234567").Value;
            var nationalCode = NationalCode.Create("1234567890").Value;

            var adminUser = User.Create(
                userName: "مدیر",
                email: email,
                passwordHash: password,
                phoneNumber: phone,
                nationalCode: nationalCode,
                roleId: adminRole.Id.ToString()
            );

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
