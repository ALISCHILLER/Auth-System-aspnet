using System;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystem.Infrastructure.Persistence.Sql.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.FirstName).HasMaxLength(128).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(128);

        builder.Property(x => x.Status).HasConversion<int>();

        builder.Property(x => x.Email)
            .HasConversion(
                email => email?.Value,
                value => value is null ? null : Email.Create(value))
            .HasMaxLength(320);

        builder.Property(x => x.PhoneNumber)
            .HasConversion(
                number => number?.Value,
                value => value is null ? null : PhoneNumber.Create(value))
            .HasMaxLength(32);

        builder.Property(x => x.NationalCode)
            .HasConversion(
                nationalCode => nationalCode?.Value,
                value => value is null ? null : NationalCode.Create(value))
            .HasMaxLength(32);

        builder.Property(x => x.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.CreateFromHash(value))
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(x => x.TwoFactorSecretKey)
            .HasConversion(
                secret => secret?.Value,
                value => value is null ? null : TwoFactorSecretKey.CreateFromExisting(value, "AuthSystem", isActive: true, DateTime.UtcNow))
            .HasMaxLength(256);

        builder.Property(x => x.LastLoginAt);
        builder.Property(x => x.LockoutEnd);
        builder.Property(x => x.AccessFailedCount);
        builder.Property(x => x.IsEmailVerified);
        builder.Property(x => x.IsPhoneVerified);
        builder.Property(x => x.IsTwoFactorEnabled);

        builder.Ignore(x => x.Roles);
        builder.Ignore(x => x.SocialLogins);
    }
}