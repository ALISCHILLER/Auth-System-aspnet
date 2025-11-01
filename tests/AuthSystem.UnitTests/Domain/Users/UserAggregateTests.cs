using System;
using System.Linq;
using AuthSystem.Domain.Common.Testing;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Entities.UserAggregate.Events;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;

namespace AuthSystem.UnitTests.Domain.Users;

public sealed class UserAggregateTests : DomainTestBase
{
    [Fact]
    public void ChangeEmail_RaisesEventAndResetsVerification()
    {
        // Arrange
        var user = CreateUser(emailVerified: true);
        var newEmail = Email.Create("new.email@example.com");

        // Act
        user.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail.Value, user.Email?.Value);
        Assert.False(user.IsEmailVerified);

        var domainEvent = Assert.Single(user.DomainEvents.OfType<UserEmailChangedEvent>());
        Assert.Equal(newEmail.Value, domainEvent.NewEmail.Value);
        Assert.True(domainEvent.WasPreviouslyVerified);
        Assert.Equal(user.Id, domainEvent.UserId);
    }

    [Fact]
    public void SetUsername_RaisesEventOnlyWhenChanged()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.SetUsername("john.doe");

        // Assert
        Assert.Equal("john.doe", user.Username);
        var domainEvent = Assert.Single(user.DomainEvents.OfType<UsernameChangedEvent>());
        Assert.Null(domainEvent.PreviousUsername);
        Assert.Equal("john.doe", domainEvent.NewUsername);
    }

    [Fact]
    public void SetUsername_DoesNotRaiseEventForSameValue()
    {
        // Arrange
        var user = CreateUser();
        user.SetUsername("john.doe");
        user.ClearDomainEvents();

        // Act
        user.SetUsername("john.doe");

        // Assert
        Assert.Empty(user.DomainEvents);
    }

    [Fact]
    public void VerifyPhone_RaisesEventOnce()
    {
        // Arrange
        var user = CreateUser(phoneVerified: false);
        var phoneNumber = user.PhoneNumber ?? throw new InvalidOperationException("Phone number must be present for this test.");

        // Act
        user.VerifyPhone();

        // Assert
        Assert.True(user.IsPhoneVerified);
        var domainEvent = Assert.Single(user.DomainEvents.OfType<UserPhoneVerifiedEvent>());
        Assert.Equal(phoneNumber.Value, domainEvent.PhoneNumber.Value);
    }

    [Fact]
    public void ResetAccessFailedCount_RaisesEventWhenCounterPositive()
    {
        // Arrange
        var user = CreateUser();
        user.RegisterLoginFailure("invalid password");
        user.ClearDomainEvents();

        // Act
        user.ResetAccessFailedCount();

        // Assert
        Assert.Equal(0, user.AccessFailedCount);
        var domainEvent = Assert.Single(user.DomainEvents.OfType<UserAccessFailedCountResetEvent>());
        Assert.Equal(1, domainEvent.PreviousFailedCount);
    }

    [Fact]
    public void AddSocialLogin_RaisesEventAndStoresProvider()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.AddSocialLogin("google", "google-user-id");

        // Assert
        Assert.Equal("google-user-id", user.SocialLogins["google"]);
        var domainEvent = Assert.Single(user.DomainEvents.OfType<UserSocialLoginLinkedEvent>());
        Assert.Equal("google", domainEvent.Provider);
        Assert.Equal("google-user-id", domainEvent.ProviderUserId);
    }

    [Fact]
    public void AddSocialLogin_ThrowsDomainExceptionForMissingProvider()
    {
        // Arrange
        var user = CreateUser();

        // Act & Assert
        Assert.Throws<InvalidSocialLoginException>(() => user.AddSocialLogin(" ", "provider-id"));
    }

    private static User CreateUser(bool emailVerified = true, bool phoneVerified = true)
    {
        var email = Email.Create("john.doe@example.com");
        var passwordHash = PasswordHash.CreateFromPlainText("SecurePassword123!");
        var phoneNumber = PhoneNumber.Create("09121234567");

        var user = new User(
            Guid.NewGuid(),
            email,
            passwordHash,
            "John",
            "Doe",
            phoneNumber,
            dateOfBirth: null,
            nationalCode: null,
            isEmailVerified: emailVerified,
            isPhoneVerified: phoneVerified,
            isSocialLogin: false);

        user.ClearDomainEvents();
        return user;
    }
}