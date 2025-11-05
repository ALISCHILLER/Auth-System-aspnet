using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.PermissionType)
            .HasConversion<int>()
            .IsRequired();
        builder.HasIndex(x => new { x.RoleId, x.PermissionType }).IsUnique();
    }
}