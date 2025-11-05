using AuthSystem.Domain.Entities.Authorization.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(256).IsRequired();
        builder.Property(x => x.IsDefault);
        builder.Property(x => x.IsSystemRole);

        builder.HasMany<RolePermission>("_permissions")
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<UserRole>("_userRoles")
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_permissions").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation("_userRoles").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}