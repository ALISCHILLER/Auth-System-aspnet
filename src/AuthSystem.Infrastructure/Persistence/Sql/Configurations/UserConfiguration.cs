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
        builder.HasIndex(x => x.Username)
         .IsUnique()
         .HasFilter("[Username] IS NOT NULL");

        builder.Property(x => x.Status).HasConversion<int>();

        builder.Property(x => x.Email)
            .HasConversion(
               email => email == null ? null : email.Value,
                value => value == null ? null : Email.Create(value))
            .HasMaxLength(320);
        builder.HasIndex(x => x.Email)
           .IsUnique()
           .HasFilter("[Email] IS NOT NULL");

        builder.Property(x => x.PhoneNumber)
            .HasConversion(
                number => number == null ? null : number.Value,
                value => value == null ? null : PhoneNumber.Create(value))
            .HasMaxLength(32);
        builder.HasIndex(x => x.PhoneNumber)
          .IsUnique()
          .HasFilter("[PhoneNumber] IS NOT NULL");

        builder.Property(x => x.NationalCode)
            .HasConversion(
                  nationalCode => nationalCode == null ? null : nationalCode.Value,
                value => value == null ? null : NationalCode.Create(value))
            .HasMaxLength(32);

        builder.Property(x => x.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.CreateFromHash(value, createdAt: null))
            .HasMaxLength(512)
            .IsRequired();

        builder.OwnsOne(x => x.TwoFactorSecretKey, navigationBuilder =>
        {
            navigationBuilder.Property(secret => secret.Value)
                .HasColumnName("TwoFactorSecretKey")
                .HasMaxLength(256)
                .IsRequired(false);

            navigationBuilder.Property(secret => secret.Issuer)
                .HasColumnName("TwoFactorSecretKeyIssuer")
                .HasMaxLength(256)
                .IsRequired(false);

            navigationBuilder.Property(secret => secret.CreatedAt)
                .HasColumnName("TwoFactorSecretKeyCreatedAt")
                .HasColumnType("datetime2(0)")
                .IsRequired(false);

            navigationBuilder.Property(secret => secret.IsActive)
                .HasColumnName("TwoFactorSecretKeyIsActive")
                .HasColumnType("bit")
                .IsRequired(false);

            navigationBuilder.Property(secret => secret.LastUsedAt)
                .HasColumnName("TwoFactorSecretKeyLastUsedAt")
                .HasColumnType("datetime2(0)")
                .IsRequired(false);

            navigationBuilder.WithOwner();
        });

        builder.Navigation(x => x.TwoFactorSecretKey).IsRequired(false);

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