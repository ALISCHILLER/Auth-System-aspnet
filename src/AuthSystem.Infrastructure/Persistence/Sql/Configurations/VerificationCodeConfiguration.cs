using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Infrastructure.Verification.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCodeEntity>
{
    public void Configure(EntityTypeBuilder<VerificationCodeEntity> builder)
    {
        builder.ToTable("VerificationCodes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CodeHash).HasMaxLength(88).IsRequired();
        builder.Property(x => x.ExpiresAtUtc).HasColumnType("datetime2(0)");
        builder.Property(x => x.ConsumedAtUtc).HasColumnType("datetime2(0)");
        builder.HasIndex(x => new { x.UserId, x.ExpiresAtUtc });
        builder.HasIndex(x => new { x.UserId, x.CodeHash });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}