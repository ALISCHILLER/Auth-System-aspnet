using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Auditing;

internal sealed class SecurityEventLogConfiguration : IEntityTypeConfiguration<SecurityEventLog>
{
    public void Configure(EntityTypeBuilder<SecurityEventLog> builder)
    {
        builder.ToTable("SecurityEventLogs");
        builder.HasKey(log => log.Id);

        builder.Property(log => log.EventType)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(log => log.UserName)
            .HasMaxLength(256);

        builder.Property(log => log.TenantId)
            .HasMaxLength(128);

        builder.Property(log => log.IpAddress)
            .HasMaxLength(64);

        builder.Property(log => log.UserAgent)
            .HasMaxLength(512);

        builder.Property(log => log.Description)
            .HasMaxLength(1024);

        builder.Property(log => log.MetadataJson)
            .HasColumnType("nvarchar(max)");
    }
}