using System;
using AuthSystem.Infrastructure.Security;
using AuthSystem.Infrastructure.Time;

namespace AuthSystem.UnitTests.Infrastructure.Security;

public class SecurityServiceTests
{
    [Fact]
    public void PasswordHasher_Generates_Verifiable_Hash()
    {
        var hasher = new AspNetPasswordHasher();

        var hash = hasher.Hash("super-secret");

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.True(hasher.Verify("super-secret", hash));
        Assert.False(hasher.Verify("wrong-password", hash));
    }

    [Fact]
    public void SystemDateTimeProvider_Returns_Utc()
    {
        var provider = new SystemDateTimeProvider();

        var utcNow = provider.UtcNow;
        var difference = Math.Abs((utcNow - DateTime.UtcNow).TotalSeconds);

        Assert.True(difference < 1, "SystemDateTimeProvider should return DateTime.UtcNow");
        Assert.Equal(DateTimeKind.Utc, utcNow.Kind);
    }
}