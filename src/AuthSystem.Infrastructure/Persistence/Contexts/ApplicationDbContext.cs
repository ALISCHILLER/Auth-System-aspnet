using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.Security.LoginHistory;
using AuthSystem.Domain.Entities.Security.UserDevice;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// کلاس اصلی DbContext برای سیستم احراز هویت
    /// این کلاس تمام DbSetها و تنظیمات مدل را مدیریت می‌کند
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // تعریف DbSetها برای موجودیت‌های دامنه
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }

        /// <summary>
        /// پیکربندی مدل‌ها هنگام ساخت مدل EF Core
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // بارگذاری و اعمال همه پیکربندی‌های موجود در اسمبلی جاری
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // درج داده‌های اولیه
            SeedInitialData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// ذخیره تغییرات در دیتابیس همراه با پردازش رویدادهای دامنه
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // پردازش رویدادهای دامنه قبل از ذخیره تغییرات
            await DispatchDomainEventsAsync();

            // ذخیره تغییرات در پایگاه داده
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// استخراج و پردازش رویدادهای دامنه تمام موجودیت‌های تغییر یافته
        /// </summary>
        private async Task DispatchDomainEventsAsync()
        {
            // گرفتن موجودیت‌هایی که رویداد دامنه دارند
            var entitiesWithEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToArray();

                // پاک کردن رویدادها از موجودیت تا از پردازش دوباره جلوگیری شود
                entity.ClearDomainEvents();

                // TODO: ارسال رویدادها به Event Dispatcher یا Mediator (مثلاً MediatR)
                // اینجا می‌توانید رویدادها را ارسال کنید، مثلاً:
                // foreach (var domainEvent in domainEvents)
                //     await _mediator.Publish(domainEvent);

                // فعلاً بدون پیاده‌سازی خاص (سینک یا آسنک)
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// درج داده‌های اولیه (Seed)
        /// </summary>
        private static void SeedInitialData(ModelBuilder modelBuilder)
        {
            // تعریف شناسه‌های ثابت برای نقش‌ها (GUIDهای ثابت برای ثبات داده)
            var adminRoleId = Guid.Parse("A9A9A9A9-A9A9-A9A9-A9A9-A9A9A9A9A9A9");
            var userRoleId = Guid.Parse("B9B9B9B9-B9B9-B9B9-B9B9-B9B9B9B9B9B9");

            // نمونه‌های اولیه Role به صورت anonymous object برای HasData
            modelBuilder.Entity<Role>().HasData(
                new
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    Description = "ادمین سیستم",
                    CreatedAt = System.DateTime.UtcNow,
                    UpdatedAt = (System.DateTime?)null
                },
                new
                {
                    Id = userRoleId,
                    Name = "User",
                    Description = "کاربر عادی",
                    CreatedAt = System.DateTime.UtcNow,
                    UpdatedAt = (System.DateTime?)null
                }
            );

            // TODO: اگر داده‌های اولیه دیگر نیاز است اینجا اضافه شود
        }
    }
}
