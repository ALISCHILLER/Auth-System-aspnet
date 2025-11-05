using AuthSystem.Domain.Entities.Authorization.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(128).IsRequired();
        builder.Property(x => x.RoleName).HasMaxLength(128).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
    }
}